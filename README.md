# ContensiveMVC
Microsoft MVC examples with Contensive integrations

This is a simple example of a .Net MVC Framework integration of Contensive. MVC calls Contensive Addons when referenced by route.

## Instructions

### Download and install the Contensive CLI (Windows only)
  Go to http://contensive.io/downloads and download then run the CLI installation

### Configure the resources for this instance of the Contensive server
  Open a command prompt
  
  cd \program files (x86)\kma\Contensive5
  
  cc --configure
  
    ( enter remote resources as needed )
    
  cc -n MVCDemo
  
    ( creates a default site named MVCDemo. Leave the name off the command to manually configure folders, etc)
   
### Create this Project From Scratch (skip if you are just using this project)

  Open Visual Studio 2017
  
  New Project
  
  Select ASP.NET Web Application (.Net Framework)
  
  Include MVC resources and (optionally) Web API resources
  
  When the demo is loaded, you can run this code as it is the standard Microsoft MVC demo.
  
  Open project properties and change the build to 4.7.2
  
  Open Nuget and install Contensive.Processor (which installes Contensive.CPBase as dependency)
  
  In the app-start, open routeConfig and add the following code to add the Contensive routes to the MVC application
    
    //
    // -- add dynamic routes from Contensive add-ons and link-alias
    // -- the Contensive app can have any name, but the sitename included here as an option
    using (var cp = new  Contensive.Processor.CPClass(System.Web.Hosting.HostingEnvironment.SiteName)) {
        foreach (var kvp in cp.routeMap.routeDictionary) {
            try {
                routes.MapRoute(
                    name: kvp.Value.virtualRoute,
                    url: kvp.Value.virtualRoute,
                    defaults: new { controller = "Home", action = "Contensive" }
                );
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex, "Unexpected exception adding virtualRoute [" + kvp.Key + "]");
            }
        }
    }
    
  In the HomeController add the following to execute the Contensive routes
    
    //
    // -- Contensive addons
    [ValidateInput(false)]
    public ActionResult Contensive() {
        using (var cp = new Contensive.Processor.CPClass(System.Web.Hosting.HostingEnvironment.SiteName, System.Web.HttpContext.Current)) {
            return Content(cp.executeRoute());
        }
    }
    
  IN the web.config, add the following so allow Contensive routes to save html
  
    <system.web>
      <httpRuntime targetFramework="4.6.1" requestValidationMode="2.0"  />
    </system.web>

  Use IISExpress to open the site from a temp server to test the new site. Go to /admin to see the Contensive admin site
  
  Publish the site to IIS




## Current Issues

1) Clearscript DLL error

- remove ClearScriptV8-32.dll and ClearScriptV8-64.dll from the /bin folder

2) When using IISExpress to demo the site, the /admin addon runs fine, but addons that it runs that install assets in the wwwroot will not be able to access those assets.

- publish the site and import it into an website
- I will modify all
