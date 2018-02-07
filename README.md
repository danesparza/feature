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

These tools help you use your existing configuration system to see if a feature is enabled for a given user/group/network, see what variant (if any) a user should get, and get & store Feature flags as JSON data.

It doesn't matter if your config system is [built in to .NET](https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.appsettings(v=vs.110).aspx), uses tools like [Consul](https://www.consul.io/api/kv.html) or [Centralconfig](https://github.com/cagedtornado/centralconfig), or is custom built -- you can use this for feature flags.

In short: Store your feature flags in your configuration system as a JSON string and use these utilities to interact with your feature flags.

#### See if a feature is enabled

A simple example
```csharp
using Feature.Library;

if(Feature.IsEnabled(featureFlag))
{
  //  The feature is enabled.
  //  Perform whatever needs to be done for the feature
}
```

A more complicated example showing all possible parameters
```csharp
using Feature.Library;

//  Read in from your configuration system somewhere...
var featureFlag = new FeatureFlag{ Users = new List<string>{ "iserra", "MReynolds"} };

//  Check the flag to see if our current user/group/privs/network mean that the feature is on or off
//  (Don't worry:  almost all of these parameters are optional)
if(Feature.IsEnabled(featureFlag, testUser, testGroup, testUrl, testInternal, testAdmin))
{
  //  The feature is enabled.
  //  Perform whatever needs to be done for the feature

}

//  You can have an else block here...
//  but more likely you'll just proceed as normal

```

#### Get a feature flag from a JSON string

```csharp
using Feature.Library;

string jsonString = "{\"enabled\": true}";
FeatureFlag retval = jsonString.ToFeatureFlag();
```

#### Store a feature flag to a JSON string

```csharp
using Feature.Library;

var flag = new FeatureFlag{ Internal = true, Admin = true };
string jsonString = flag.ToJSON();
```

## JSON format

All available options:

```JSON
{
    "enabled": "true",
    "users": [
        "user1",
        "user2"
    ],
    "groups": [
        "group1",
        "group2"
    ],
    "percent_loggedin": 42,
    "admin": true,
    "internal": true
}
```

#### Examples

Completely on
```JSON
{ "enabled": true}
```

Completely off
```JSON
{ "enabled": false}
```

On for admin
```JSON
{ "admin": true}
```

On for internal users/employees
```JSON
{ "internal": true}
```

On for certain users
```JSON
{
    "users": [
        "user1",
        "user2"
    ]
}
```

On for certain groups
```JSON
{
    "groups": [
        "group1",
        "group2"
    ]
}
```

On for a percentage of logged in users (0 being no users, 100 being all of them)
```JSON
{ "percent_loggedin": 42}
```

On for both admin and for certain users
```JSON
{
    "users": [
        "user1",
        "user2"
    ],
    "admin": true
}
```

