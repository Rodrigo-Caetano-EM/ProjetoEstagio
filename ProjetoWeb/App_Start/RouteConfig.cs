namespace ProjetoWeb.App_Start
{
    public class RouteConfig
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();

            app.UseMiddleware<IgnoreRouteMiddleware>();

            app.UseStaticFiles();
            app.UseMvc();
        }
    }

    public class IgnoreRouteMiddleware
    {
        private readonly RequestDelegate next;

        public IgnoreRouteMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("favicon.ico"))
            {

                context.Response.StatusCode = 404;

                Console.WriteLine("Ignored!");

                return;
            }

            await next.Invoke(context);
        }
    }
}
