<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SatelliteOrbit" generation="1" functional="0" release="0" Id="4a1472a9-3141-4ea5-97ca-1f84b4ae0d35" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SatelliteOrbitGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="CalculationWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SatelliteOrbit/SatelliteOrbitGroup/MapCalculationWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="CalculationWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SatelliteOrbit/SatelliteOrbitGroup/MapCalculationWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapCalculationWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SatelliteOrbit/SatelliteOrbitGroup/CalculationWorkerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapCalculationWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SatelliteOrbit/SatelliteOrbitGroup/CalculationWorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CalculationWorkerRole" generation="1" functional="0" release="0" software="C:\Users\慈行\Documents\Projects\spacebottle\SatelliteOrbit\csx\Debug\roles\CalculationWorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CalculationWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CalculationWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SatelliteOrbit/SatelliteOrbitGroup/CalculationWorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SatelliteOrbit/SatelliteOrbitGroup/CalculationWorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SatelliteOrbit/SatelliteOrbitGroup/CalculationWorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="CalculationWorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="CalculationWorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="CalculationWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>