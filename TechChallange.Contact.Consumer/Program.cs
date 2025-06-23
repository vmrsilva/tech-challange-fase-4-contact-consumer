//using TechChallange.Contact.Consumer;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();



using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechChallange.Contact.Consumer;
using TechChallange.Contact.Domain.Base.Repository;
using TechChallange.Contact.Domain.Contact.Messaging;
using TechChallange.Contact.Domain.Contact.Repository;
using TechChallange.Contact.Domain.Contact.Service;
using TechChallange.Contact.Infrastructure.Context;
using TechChallange.Contact.Infrastructure.Repository.Base;
using TechChallange.Contact.Infrastructure.Repository.Contact;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var fila = configuration.GetSection("MassTransit")["QueueCreateRegion"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["User"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactService, ContactService>();
        services.AddDbContext<TechChallangeContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<InsertContactConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });


                cfg.Message<ContactCreateMessageDto>(m =>
                {
                    m.SetEntityName("contact-insert-exchange");
                });

                cfg.ReceiveEndpoint(fila, e =>
                {
                    e.ConfigureConsumer<InsertContactConsumer>(context);
                });
            });

        });
    })
    .Build();



host.Run();
