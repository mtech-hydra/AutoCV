using Microsoft.Data.SqlClient;
using System;
using System.Reflection.Emit;
// NOTE:
// EF Core migrations intentionally disabled.
// Database schema will be introduced later.
// this comment is here to prevent "why is nothing migrating" headache?

using AutoCV.Contracts.Interfaces;
using AutoCV.Contracts.Dtos;
using AutoCV.Web.Fakes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IJobAdSource, FakeJobAdSource>();
builder.Services.AddSingleton<ICandidateProfileSource, FakeCandidateProfileSource>();
builder.Services.AddSingleton<ICvGenerator, FakeCvGenerator>();
builder.Services.AddSingleton<IGeneratedDocumentWriter, FakeDocumentWriter>();
builder.Services.AddSingleton<HappyPathRunner>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapGet("/happy", async (HappyPathRunner runner) =>
{
    await runner.RunAsync("job-123");
    return Results.Ok("Happy path executed");
});

app.MapGet("/health/db", async () =>
{
    var connStr = app.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connStr))
        return Results.Problem("Connection string not configured");

    await using var conn = new SqlConnection(connStr);
    await conn.OpenAsync();

    return Results.Ok("DB OK");
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
