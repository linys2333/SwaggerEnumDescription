## 一些说明

> 在Startup中添加SwaggerEnumDescription

``` C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Info
        {
            Version = "v1",
            Title = "API V1"
        });
        
        c.DocumentFilter<SwaggerEnumDescription>();

        c.IgnoreObsoleteActions();
        c.IgnoreObsoleteProperties();
    });
}
```
