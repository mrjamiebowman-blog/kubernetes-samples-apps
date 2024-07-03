var builder = DistributedApplication.CreateBuilder(args);

// projects
builder.AddProject<Projects.MrJB_Kubernetes_Customers_Api>("api-customers");
builder.AddProject<Projects.MrJB_Kubernetes_Orders_Api>("api-orders");
builder.AddProject<Projects.MrJB_Kubernetes_IdentityServer>("app-identityserver");
builder.AddProject<Projects.MrJB_Kubernetes_Customers_Consumer>("consumer-customers");

builder.Build().Run();
