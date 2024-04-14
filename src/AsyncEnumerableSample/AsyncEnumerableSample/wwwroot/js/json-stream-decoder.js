"use strict";
class JsonStreamDecoder {
    constructor() {
        this.level = 0;
        this.partialItem = '';
        this.decoder = new TextDecoder();
    }
    
    decodeChunk(value) {
        const chunk = this.decoder.decode(value);
        let itemStart = 0;
        for (let i = 0; i < chunk.length; i++) {
            if (chunk[i] === JTOKEN_START_OBJECT) {
                if (this.level === 0) {
                    itemStart = i;
                }
                this.level++;
            }
            if (chunk[i] === JTOKEN_END_OBJECT) {
                this.level--;
                if (this.level === 0) {
                    let item = chunk.substring(itemStart, i + 1);
                    if (this.partialItem) {
                        item = this.partialItem + item;
                        this.partialItem = '';
                    }

                    return JSON.parse(item);
                }
            }
        }
        if (this.level !== 0) {
            this.partialItem = chunk.substring(itemStart);
        }
    }
}

const JTOKEN_START_OBJECT = '{';
const JTOKEN_END_OBJECT = '}';

window.fetchAsyncEnumerable = async (dotnetHelper) => {
    const url = "/api/async-enumerable";
    const response = await fetch(url);
    const reader = response.body.getReader();

    if (!reader) {
        throw new Error('Failed to read response');
    }

    const decoder = new JsonStreamDecoder();

    while (true) {
        const { done, value } = await reader.read();
        if (done) break;
        if (!value) continue;
        const chunk = decoder.decodeChunk(value);
        const isOk = await dotnetHelper.invokeMethodAsync('OnDataReceived', chunk);
        console.log({isOk})
        if (!isOk) {
            break;
        }
    }

    reader.releaseLock();
}