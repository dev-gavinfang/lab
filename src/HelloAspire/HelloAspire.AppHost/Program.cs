var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("gavin-redis");

var orleans = builder.AddOrleans("default")
    .WithClustering(redis)
    .WithGrainStorage(redis)
    .WithMemoryGrainStorage("default");

builder.AddProject<Projects.SiloServer>("silo")
       .WithReference(orleans)
       .WithReplicas(2);

var apiService = builder.AddProject<Projects.HelloAspire_ApiService>("apiservice")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.HelloAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WithReference(apiService)
    .WithReference(orleans.AsClient())
    .WithReplicas(1);
    
builder.Build().Run();
