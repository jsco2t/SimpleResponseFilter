# SimpleResponseFilter
SimpleResponseFilter is a managed IIS module used for performing simple http header response filtering. 

## Warnings and Disclaimers
This code is published as an example/weekend learning project. 

As noted in the license file:
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

If you choose to use this module - you do so at your own risk :)

## Building the project + module:
IIS `managed` filter modules typically need to be installed into the GAC
for IIS to be able to load them. This requires:

1. The filter module to be strongly typed.
2. The filter module to be installed into the GAC using gacutil or an installer.

The simple way of handling this is to have the project build an installer (which is included 
in the solution for SimpleResponseFilter)

Steps for setting up the project for building:

1. Clone repo locally.
2. Open the solution in VS 2015 (or higher)
3. Right click on the project `SimpleResponseFilterModule` and choose `properties`
4. In the `properties` dialog select the `signing` tab and setup a `SNK` file for the assembly
5. Rebuild the project.
6. Rebuilt the installer project: `SimpleResponseFilterInstaller`

## Configuring IIS (7.5 and up) to use the filter:

The filter can be manually loaded into a specific web site by adjusting that sites web.config file. 
Add something similar to the example below under the `configuration` node. Note that `system.web` and `modules` 
may already exist in your web.config file.
```
<system.webServer>
  <modules>
    <add name="SimpleResponseFilterModule" type="SimpleResponseFilter.FilterModule, SimpleResponseFilterModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1234matchesyourassembly56789" />
  </modules>
</system.webServer>
```
