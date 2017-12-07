# feature [![Build status](https://ci.appveyor.com/api/projects/status/e8b6qyp05k3jk730?svg=true)](https://ci.appveyor.com/project/danesparza/feature) [![NuGet](https://img.shields.io/nuget/v/Feature.svg)](https://www.nuget.org/packages/Feature/)

Feature flag helpers (based on [etsy/feature](http://github.com/etsy/feature) and reddit's [config/feature](https://github.com/reddit/reddit/tree/master/r2/r2/config/feature))

### Quick Start

Install the [NuGet package](https://www.nuget.org/packages/Feature/) from the package manager console:

```powershell
Install-Package Feature
```

### What are feature flags?

Think of feature flags as another configuration item for your app with some special sauce.  You can guard your code with a feature flag check and then turn parts of your code on/off based on the feature flag status.  

From [etsy's docs](https://github.com/etsy/feature#feature-api) on the subject:

> The Feature API is how we selectively enable and disable features at a very fine grain as well as enabling features for a percentage of users for operational ramp-ups and for A/B tests. A feature can be completely enabled, completely disabled, or something in between and can comprise a number of related variants.

### How do I use this?

These tools help you use your existing configuration system (either the [built in to .NET](https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.appsettings(v=vs.110).aspx) or tools like [Consul](https://www.consul.io/api/kv.html), [Centralconfig](https://github.com/cagedtornado/centralconfig), or custom built)

Store your feature flags in your configuration system as a JSON string and use these utilities to interact with your feature flags.

#### Get a Feature Flag from JSON string

```csharp
using Feature.Library;

string jsonString = "{\"enabled\": true}";
var retval = jsonString.ToFeatureFlag();
```
