# SimpleResponseFilter
SimpleResponseFilter is a managed IIS module used for performing simple http header response filtering. 

## Warnings and Disclaimers
This code is published as an example/weekend learning project. 

As noted in the license file this software *and* this help documentation is
provided with the following disclaimer:

*THE SOFTWARE (AND ASSOCIATED DOCUMENTATION)  IS PROVIDED "AS IS", WITHOUT 
WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.*

If you choose to use this module - you do so at your own risk :)

## Building the project + module:
IIS `managed` filter modules typically need to be installed into the GAC
for IIS to be able to load them. This requires:

1. The filter module to be strongly typed.
2. The filter module to be installed into the GAC using gacutil or an installer.

The simple way of handling this is to have the project build an installer (which is included 
in the solution for `SimpleResponseFilter`)

Steps for setting up the project for building:

1. Clone repo locally.
2. Open the solution in VS 2015 (or higher)
3. Right click on the project `SimpleResponseFilterModule` and choose `properties`
4. In the `properties` dialog select the `signing` tab and setup a `SNK` file for the assembly
5. Rebuild the project.
6. Rebuilt the installer project: `SimpleResponseFilterInstaller`

## Configuring IIS (7.5 and up) to use the filter:

#### Step 1: Run the installer - this will add the module to the GAC on the machine.

#### Step 2: Install the module into IIS. 

The simple option for this is just to add `SimpleResponseFilter` to IIS directly. Alternatively (discussed below) 
SimpleResponseFilter can be add to individual websites/webapplications using a `web.config` file.

To add the module globally for all sites in IIS - run something similar to the following. 
The `publickeytoken` will need to have a value which matches the strong-named assembly 
which was installed:

```
.\appcmd.exe add module /name:"SimpleResponseFilterModule" /type:"SimpleResponseFilter.FilterModule, SimpleResponseFilterModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1234matchesyourassembly56789"
```

Alternatively the filter can be manually loaded into a specific web site by adjusting that sites web.config file.
To do this, add something similar to the example below under the `configuration` node. Note that `system.webServer` and `modules` 
may already exist in a web.config file. As noted above the `PublicKeyToken` should have a value which matches the
assembly that was installed into the GAC:

```
<system.webServer>
  <modules>
    <add name="SimpleResponseFilterModule" type="SimpleResponseFilter.FilterModule, SimpleResponseFilterModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1234matchesyourassembly56789" />
  </modules>
</system.webServer>
```

### Step 3: Adjust which headers to filter.

`SimpleResponseFilter` will look at the `appSettings` configuration key `unsupportedHeaders` to 
determine which headers to remove. If none are specified then SimpleResponseFilter will default
to filtering the following set: `Server,X-AspNetMvc-Version,X-Powered-By,X-AspNet-Version`

`unsuppotedHeaders` can either be added using the IIS Management UI, or can be added 
using `appcmd.exe`. The following shows how to add/remove `unsupportedHeaders` using
`appcmd.exe` Please note that the *value* for `unsupportedHeaders` MUST be a comma seperated
list with no spaces.

To add/remove `unsupportedHeaders` to all websites running in IIS:
```
To Add:
.\appcmd.exe set config /section:appSettings MACHINE/WEBROOT /+"[key='unsupportedHeaders', value='Server,X-AspNetMvc-Version,X-Powered-By,X-AspNet-Version']"

To Remove:
.\appcmd.exe set config /section:appSettings MACHINE/WEBROOT /-"[key='unsupportedHeaders', value='Server,X-AspNetMvc-Version,X-Powered-By,X-AspNet-Version']"
```

To add/remove `unsupportedHeaders` to a specific website:
```
To Add:
.\appcmd.exe set config /section:appSettings "Default Web Site" /+"[key='unsupportedHeaders', value='Server,X-AspNetMvc-Version,X-Powered-By,X-AspNet-Version']"

To Remove:
.\appcmd.exe set config /section:appSettings "Default Web Site" /-"[key='unsupportedHeaders', value='Server,X-AspNetMvc-Version,X-Powered-By,X-AspNet-Version']"
```

### NOTES: 

- IIS Configurations are inherited. Adding `unsupportedHeaders` globally 
and then to a specific web site will cause configuration errors. `unsupportedHeaders` 
should be added globally, or only to those sites that `SimpleResponseFilter` has been added to.

- Depending on how an IIS instance is configured the header field `X-Powered-By` may be added back
by IIS *after* filtering has happened. This header can be removed by adjusting the "HTTP Response 
Headers" setting in IIS.
