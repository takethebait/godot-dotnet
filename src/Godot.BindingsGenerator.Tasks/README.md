# Godot.BindingsGenerator.Tasks

MSBuild task to generate bindings for Godot contained in the [Godot.Bindings](../Godot.Bindings) package, using the [Godot.BindingsGenerator](../Godot.BindingsGenerator) library.

## Usage

Add a `UsingTask` declaration pointing at the task assembly, then invoke the `GenerateTask` in a target:

```xml
<UsingTask TaskName="Godot.BindingsGenerator.GenerateTask" AssemblyFile="path\to\Godot.BindingsGenerator.Tasks.dll" TaskFactory="TaskHostFactory" />

<Target Name="GenerateGodotBindings" BeforeTargets="BeforeCompile">
  <GenerateTask ExtensionApiPath="path\to\extension_api.json"
                ExtensionInterfacePath="path\to\gdextension_interface.h"
                OutputPath="$(MSBuildThisFileDirectory)Generated" />
  <ItemGroup>
    <Compile Include="$(MSBuildThisFileDirectory)Generated\**\*.cs" />
  </ItemGroup>
</Target>
```
