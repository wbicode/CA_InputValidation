# WiX Custom Action - Input Validation

This is a CustomAction for a WiX Project (v3) which provides input sanitizer utils <br />

* Check if a Port is available on this machine
* Check if a given windows service is installed
* Replace backslashes with frontslashes
  * for example when using a path in the command line (if it ends with a backslash the following character could be escaped)
* Replace frontslashes with backslashes
  * for example when using user input as a WiX path for a directory (can only consist of backslashes)
* pretty format .xml files

# Usage

Include the built CA_InputValidation.CA.dll from this project in your own project or use the published nuget (defined in CA_InputValidation/CA_InputValidation.nuspec). <br />

Reference the .dll in your .wxs like so:

```xml
<Binary Id="CA_InputValidation" SourceFile="pathToPackages/CA_InputValidation.CA.dll" />
```

* "pathToNugetPackages" could be $(var.SolutionDir)/packages

<br />

## Check if port is available

*CustomAction*:

```xml
<CustomAction Id="CheckPort" BinaryKey="CA_InputValidation" DllEntry="CheckPort" Execute="immediate" />
```



Somewhere below a button:

```xml
   <Publish Property="PORT_TO_CHECK" Value="[SERVICE_PORT]" Order="1">1</Publish>
   <Publish Event="DoAction" Value="CheckPort" Order="2">1</Publish>
   <Publish Event="SpawnDialog" Value="PortInUseWarningDlg" Order="3">NOT PORT_IS_AVAILABLE</Publish>
```

## Replace backslashes with frontslashes

```xml
   <SetProperty Before="ReplaceLOG_PATH" Id="PROPERTY_TO_REPLACE_BACKSLASH" Value="LOG_PATH" Sequence="execute" />
   <CustomAction Id="ReplaceLOG_PATH" DllEntry="ReplaceBackslashWithFrontslash" BinaryKey="CA_InputValidation" />
```

* the property PROPERTY_TO_REPLACE_BACKSLASH will be set to LOG_PATH before the CustomAction ReplaceLOG_PATH gets executed (to allow multiple uses of this functionality)

And you have to schedule your CustomAction in your InstallExecuteSequence (or InstallUIExecuteSequence)

```xml
<InstallExecuteSequence>
  <Custom Action="ReplaceLOG_PATH" Before="InstallTS">NOT Installed</Custom>
</InstallExecuteSequence>
```

## Replace frontslashes with backslashes

For example when using user input as a WiX path for a directory (which can only consist of backslashes)

```xml
   <SetProperty Before="ReplaceLOG_PATH" Id="PROPERTY_TO_REPLACE_FRONTSLASH" Value="USER_LOG_PATH" Sequence="execute" />
   <CustomAction Id="ReplaceLOG_PATH" DllEntry="ReplaceFrontslashWithBackslash" BinaryKey="CA_InputValidation" />
```

```