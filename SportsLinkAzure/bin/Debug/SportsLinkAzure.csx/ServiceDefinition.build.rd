<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SportsLinkAzure" generation="1" functional="0" release="0" Id="6f31040d-1ca9-4aaf-806a-fd31cbe220ac" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SportsLinkAzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SportsLinkWeb:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/LB:SportsLinkWeb:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="SportsLinkWeb:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/MapSportsLinkWeb:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="SportsLinkWeb:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/MapSportsLinkWeb:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="SportsLinkWeb:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/MapSportsLinkWeb:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="SportsLinkWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/MapSportsLinkWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SportsLinkWebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/MapSportsLinkWebInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:SportsLinkWeb:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWeb/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapSportsLinkWeb:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWeb/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapSportsLinkWeb:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWeb/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapSportsLinkWeb:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWeb/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapSportsLinkWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWeb/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSportsLinkWebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWebInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SportsLinkWeb" generation="1" functional="0" release="0" software="C:\Users\nclark\Documents\Visual Studio 2010\Projects\SportsLink\SportsLinkAzure\bin\Debug\SportsLinkAzure.csx\roles\SportsLinkWeb" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SportsLinkWeb&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SportsLinkWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWebInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="SportsLinkWebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="2dcc4dca-ff83-4bf5-abfe-e3560b725553" ref="Microsoft.RedDog.Contract\ServiceContract\SportsLinkAzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="802207c2-134e-4caa-83d0-a57c6fc70766" ref="Microsoft.RedDog.Contract\Interface\SportsLinkWeb:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/SportsLinkAzure/SportsLinkAzureGroup/SportsLinkWeb:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>