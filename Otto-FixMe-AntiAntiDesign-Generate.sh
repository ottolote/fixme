#!/bin/sh
targetDir=$(pwd)
echo "My directory is $targetDir"
dotnet new list method.interface -v q
retVal=$?
if [ $retVal -ne 0 ]; then
	echo "No template found for method.interface. To use the default templates run 'dotnet new install SoEx.Method.Templates'"
	exit $retVal
fi
dotnet new list method.service -v q
retVal=$?
if [ $retVal -ne 0 ]; then
	echo "No template found for method.service. To use the default templates run 'dotnet new install SoEx.Method.Templates'"
	exit $retVal
fi
if [ ! -r FixMe.sln ] && [ ! -r FixMe.slnx ]; then
dotnet new method.solution -n FixMe --stronglyTypedIds --vogenValueObjects
fi


slnFile="FixMe.sln"
if [ -r FixMe.slnx ]; then
    slnFile="FixMe.slnx"
fi

if [ ! -r Component/Manager/Membership/Interface/FixMe.Manager.Membership.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Manager.Membership.Interface" -o "Component/Manager/Membership/Interface"
dotnet sln $slnFile add Component/Manager/Membership/Interface/FixMe.Manager.Membership.Interface.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Membership/Interface/FixMe.Manager.Membership.Interface.csproj
if [ ! -r Component/Manager/Membership/Service/FixMe.Manager.Membership.Service.csproj ]; then
dotnet new method.service -n "FixMe.Manager.Membership.Service" -o "Component/Manager/Membership/Service"
dotnet sln $slnFile add Component/Manager/Membership/Service/FixMe.Manager.Membership.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Membership/Service/FixMe.Manager.Membership.Service.csproj
if [ ! -r Component/Manager/Maintenance/Interface/FixMe.Manager.Maintenance.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Manager.Maintenance.Interface" -o "Component/Manager/Maintenance/Interface"
dotnet sln $slnFile add Component/Manager/Maintenance/Interface/FixMe.Manager.Maintenance.Interface.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Maintenance/Interface/FixMe.Manager.Maintenance.Interface.csproj
if [ ! -r Component/Manager/Maintenance/Service/FixMe.Manager.Maintenance.Service.csproj ]; then
dotnet new method.service -n "FixMe.Manager.Maintenance.Service" -o "Component/Manager/Maintenance/Service"
dotnet sln $slnFile add Component/Manager/Maintenance/Service/FixMe.Manager.Maintenance.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Maintenance/Service/FixMe.Manager.Maintenance.Service.csproj
if [ ! -r Component/Manager/Notification/Interface/FixMe.Manager.Notification.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Manager.Notification.Interface" -o "Component/Manager/Notification/Interface"
dotnet sln $slnFile add Component/Manager/Notification/Interface/FixMe.Manager.Notification.Interface.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Notification/Interface/FixMe.Manager.Notification.Interface.csproj
if [ ! -r Component/Manager/Notification/Service/FixMe.Manager.Notification.Service.csproj ]; then
dotnet new method.service -n "FixMe.Manager.Notification.Service" -o "Component/Manager/Notification/Service"
dotnet sln $slnFile add Component/Manager/Notification/Service/FixMe.Manager.Notification.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Notification/Service/FixMe.Manager.Notification.Service.csproj
if [ ! -r Component/Manager/Tasking/Interface/FixMe.Manager.Tasking.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Manager.Tasking.Interface" -o "Component/Manager/Tasking/Interface"
dotnet sln $slnFile add Component/Manager/Tasking/Interface/FixMe.Manager.Tasking.Interface.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Tasking/Interface/FixMe.Manager.Tasking.Interface.csproj
if [ ! -r Component/Manager/Tasking/Service/FixMe.Manager.Tasking.Service.csproj ]; then
dotnet new method.service -n "FixMe.Manager.Tasking.Service" -o "Component/Manager/Tasking/Service"
dotnet sln $slnFile add Component/Manager/Tasking/Service/FixMe.Manager.Tasking.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Manager/Tasking/Service/FixMe.Manager.Tasking.Service.csproj
if [ ! -r Component/Engine/Matching/Interface/FixMe.Engine.Matching.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Engine.Matching.Interface" -o "Component/Engine/Matching/Interface"
dotnet sln $slnFile add Component/Engine/Matching/Interface/FixMe.Engine.Matching.Interface.csproj
fi
if [ ! -r Component/Engine/Matching/Service/FixMe.Engine.Matching.Service.csproj ]; then
dotnet new method.service -n "FixMe.Engine.Matching.Service" -o "Component/Engine/Matching/Service"
dotnet sln $slnFile add Component/Engine/Matching/Service/FixMe.Engine.Matching.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Engine/Matching/Service/FixMe.Engine.Matching.Service.csproj
if [ ! -r Component/Engine/Scheduling/Interface/FixMe.Engine.Scheduling.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Engine.Scheduling.Interface" -o "Component/Engine/Scheduling/Interface"
dotnet sln $slnFile add Component/Engine/Scheduling/Interface/FixMe.Engine.Scheduling.Interface.csproj
fi
if [ ! -r Component/Engine/Scheduling/Service/FixMe.Engine.Scheduling.Service.csproj ]; then
dotnet new method.service -n "FixMe.Engine.Scheduling.Service" -o "Component/Engine/Scheduling/Service"
dotnet sln $slnFile add Component/Engine/Scheduling/Service/FixMe.Engine.Scheduling.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Engine/Scheduling/Service/FixMe.Engine.Scheduling.Service.csproj
if [ ! -r Component/Engine/Policy/Interface/FixMe.Engine.Policy.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Engine.Policy.Interface" -o "Component/Engine/Policy/Interface"
dotnet sln $slnFile add Component/Engine/Policy/Interface/FixMe.Engine.Policy.Interface.csproj
fi
if [ ! -r Component/Engine/Policy/Service/FixMe.Engine.Policy.Service.csproj ]; then
dotnet new method.service -n "FixMe.Engine.Policy.Service" -o "Component/Engine/Policy/Service"
dotnet sln $slnFile add Component/Engine/Policy/Service/FixMe.Engine.Policy.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Engine/Policy/Service/FixMe.Engine.Policy.Service.csproj
if [ ! -r Component/Access/Customer/Interface/FixMe.Access.Customer.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Access.Customer.Interface" -o "Component/Access/Customer/Interface"
dotnet sln $slnFile add Component/Access/Customer/Interface/FixMe.Access.Customer.Interface.csproj
fi
if [ ! -r Component/Access/Customer/Service/FixMe.Access.Customer.Service.csproj ]; then
dotnet new method.service -n "FixMe.Access.Customer.Service" -o "Component/Access/Customer/Service"
dotnet sln $slnFile add Component/Access/Customer/Service/FixMe.Access.Customer.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Access/Customer/Service/FixMe.Access.Customer.Service.csproj
if [ ! -r Component/Access/Equipment/Interface/FixMe.Access.Equipment.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Access.Equipment.Interface" -o "Component/Access/Equipment/Interface"
dotnet sln $slnFile add Component/Access/Equipment/Interface/FixMe.Access.Equipment.Interface.csproj
fi
if [ ! -r Component/Access/Equipment/Service/FixMe.Access.Equipment.Service.csproj ]; then
dotnet new method.service -n "FixMe.Access.Equipment.Service" -o "Component/Access/Equipment/Service"
dotnet sln $slnFile add Component/Access/Equipment/Service/FixMe.Access.Equipment.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Access/Equipment/Service/FixMe.Access.Equipment.Service.csproj
if [ ! -r Component/Access/Maintenance/Interface/FixMe.Access.Maintenance.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Access.Maintenance.Interface" -o "Component/Access/Maintenance/Interface"
dotnet sln $slnFile add Component/Access/Maintenance/Interface/FixMe.Access.Maintenance.Interface.csproj
fi
if [ ! -r Component/Access/Maintenance/Service/FixMe.Access.Maintenance.Service.csproj ]; then
dotnet new method.service -n "FixMe.Access.Maintenance.Service" -o "Component/Access/Maintenance/Service"
dotnet sln $slnFile add Component/Access/Maintenance/Service/FixMe.Access.Maintenance.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Access/Maintenance/Service/FixMe.Access.Maintenance.Service.csproj
if [ ! -r Component/Access/Task/Interface/FixMe.Access.Task.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Access.Task.Interface" -o "Component/Access/Task/Interface"
dotnet sln $slnFile add Component/Access/Task/Interface/FixMe.Access.Task.Interface.csproj
fi
if [ ! -r Component/Access/Task/Service/FixMe.Access.Task.Service.csproj ]; then
dotnet new method.service -n "FixMe.Access.Task.Service" -o "Component/Access/Task/Service"
dotnet sln $slnFile add Component/Access/Task/Service/FixMe.Access.Task.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Access/Task/Service/FixMe.Access.Task.Service.csproj
if [ ! -r Component/Access/Notification/Interface/FixMe.Access.Notification.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Access.Notification.Interface" -o "Component/Access/Notification/Interface"
dotnet sln $slnFile add Component/Access/Notification/Interface/FixMe.Access.Notification.Interface.csproj
fi
if [ ! -r Component/Access/Notification/Service/FixMe.Access.Notification.Service.csproj ]; then
dotnet new method.service -n "FixMe.Access.Notification.Service" -o "Component/Access/Notification/Service"
dotnet sln $slnFile add Component/Access/Notification/Service/FixMe.Access.Notification.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Access/Notification/Service/FixMe.Access.Notification.Service.csproj
if [ ! -r Component/Access/Agreement/Interface/FixMe.Access.Agreement.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Access.Agreement.Interface" -o "Component/Access/Agreement/Interface"
dotnet sln $slnFile add Component/Access/Agreement/Interface/FixMe.Access.Agreement.Interface.csproj
fi
if [ ! -r Component/Access/Agreement/Service/FixMe.Access.Agreement.Service.csproj ]; then
dotnet new method.service -n "FixMe.Access.Agreement.Service" -o "Component/Access/Agreement/Service"
dotnet sln $slnFile add Component/Access/Agreement/Service/FixMe.Access.Agreement.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Access/Agreement/Service/FixMe.Access.Agreement.Service.csproj
if [ ! -r Component/Access/ESigning/Interface/FixMe.Access.ESigning.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Access.ESigning.Interface" -o "Component/Access/ESigning/Interface"
dotnet sln $slnFile add Component/Access/ESigning/Interface/FixMe.Access.ESigning.Interface.csproj
fi
if [ ! -r Component/Access/ESigning/Service/FixMe.Access.ESigning.Service.csproj ]; then
dotnet new method.service -n "FixMe.Access.ESigning.Service" -o "Component/Access/ESigning/Service"
dotnet sln $slnFile add Component/Access/ESigning/Service/FixMe.Access.ESigning.Service.csproj
fi
dotnet add Host/InProc/FixMe.Host.InProc.csproj reference Component/Access/ESigning/Service/FixMe.Access.ESigning.Service.csproj
if [ ! -r Component/Utility/Security/Interface/FixMe.Utility.Security.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Utility.Security.Interface" -o "Component/Utility/Security/Interface"
dotnet sln $slnFile add Component/Utility/Security/Interface/FixMe.Utility.Security.Interface.csproj
fi
if [ ! -r Component/Utility/Communication/Interface/FixMe.Utility.Communication.Interface.csproj ]; then
dotnet new method.interface -n "FixMe.Utility.Communication.Interface" -o "Component/Utility/Communication/Interface"
dotnet sln $slnFile add Component/Utility/Communication/Interface/FixMe.Utility.Communication.Interface.csproj
fi
dotnet add Component/Manager/Membership/Service/FixMe.Manager.Membership.Service.csproj reference Component/Manager/Membership/Interface/FixMe.Manager.Membership.Interface.csproj
dotnet add Component/Manager/Maintenance/Service/FixMe.Manager.Maintenance.Service.csproj reference Component/Manager/Maintenance/Interface/FixMe.Manager.Maintenance.Interface.csproj
dotnet add Component/Manager/Notification/Service/FixMe.Manager.Notification.Service.csproj reference Component/Manager/Notification/Interface/FixMe.Manager.Notification.Interface.csproj
dotnet add Component/Manager/Tasking/Service/FixMe.Manager.Tasking.Service.csproj reference Component/Manager/Tasking/Interface/FixMe.Manager.Tasking.Interface.csproj
dotnet add Component/Engine/Matching/Service/FixMe.Engine.Matching.Service.csproj reference Component/Engine/Matching/Interface/FixMe.Engine.Matching.Interface.csproj
dotnet add Component/Engine/Scheduling/Service/FixMe.Engine.Scheduling.Service.csproj reference Component/Engine/Scheduling/Interface/FixMe.Engine.Scheduling.Interface.csproj
dotnet add Component/Engine/Policy/Service/FixMe.Engine.Policy.Service.csproj reference Component/Engine/Policy/Interface/FixMe.Engine.Policy.Interface.csproj
dotnet add Component/Access/Customer/Service/FixMe.Access.Customer.Service.csproj reference Component/Access/Customer/Interface/FixMe.Access.Customer.Interface.csproj
dotnet add Component/Access/Equipment/Service/FixMe.Access.Equipment.Service.csproj reference Component/Access/Equipment/Interface/FixMe.Access.Equipment.Interface.csproj
dotnet add Component/Access/Maintenance/Service/FixMe.Access.Maintenance.Service.csproj reference Component/Access/Maintenance/Interface/FixMe.Access.Maintenance.Interface.csproj
dotnet add Component/Access/Task/Service/FixMe.Access.Task.Service.csproj reference Component/Access/Task/Interface/FixMe.Access.Task.Interface.csproj
dotnet add Component/Access/Notification/Service/FixMe.Access.Notification.Service.csproj reference Component/Access/Notification/Interface/FixMe.Access.Notification.Interface.csproj
dotnet add Component/Access/Agreement/Service/FixMe.Access.Agreement.Service.csproj reference Component/Access/Agreement/Interface/FixMe.Access.Agreement.Interface.csproj
dotnet add Component/Access/ESigning/Service/FixMe.Access.ESigning.Service.csproj reference Component/Access/ESigning/Interface/FixMe.Access.ESigning.Interface.csproj

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgaW50ZXJmYWNlIElNZW1iZXJzaGlwTWFuYWdlcg0KICAgIHsNCiAgICAgICAgVGFzazxSZWdpc3RlckFjY291bnRSZXNwb25zZT4gUmVnaXN0ZXJBY2NvdW50KFJlZ2lzdGVyQWNjb3VudFJlcXVlc3QgcmVxdWVzdCk7DQogICAgICAgIFRhc2s8Q29uZmlybVVzZXJFbWFpbFJlc3BvbnNlPiBDb25maXJtVXNlckVtYWlsKENvbmZpcm1Vc2VyRW1haWxSZXF1ZXN0IHJlcXVlc3QpOw0KICAgICAgICBUYXNrPFVwZGF0ZVVzZXJQYXNzd29yZFJlc3BvbnNlPiBVcGRhdGVVc2VyUGFzc3dvcmQoVXBkYXRlVXNlclBhc3N3b3JkUmVxdWVzdCByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxTZXRVc2VyUHJlZmVyZW5jZXNSZXNwb25zZT4gU2V0VXNlclByZWZlcmVuY2VzKFNldFVzZXJQcmVmZXJlbmNlc1JlcXVlc3QgcmVxdWVzdCk7DQogICAgICAgIFRhc2s8UGVuZGluZ1JlZ2lzdHJhdGlvbj4gQ3JlYXRlUGVuZGluZ1JlZ2lzdHJhdGlvbihDcmVhdGVQZW5kaW5nUmVnaXN0cmF0aW9uUmVxdWVzdCByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxSZXNvbHZlUGVuZGluZ1JlZ2lzdHJhdGlvblJlc3BvbnNlPiBSZXNvbHZlUGVuZGluZ1JlZ2lzdHJhdGlvbihSZXNvbHZlUGVuZGluZ1JlZ2lzdHJhdGlvblJlcXVlc3QgcmVxdWVzdCk7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Membership/Interface/IMembershipManager.cs

if [ ! -d Component/Access/Customer/Interface ];
then
mkdir  Component/Access/Customer/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5DdXN0b21lci5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgaW50ZXJmYWNlIElDdXN0b21lckFjY2Vzcw0KICAgIHsNCiAgICAgICAgVGFzazxDdXN0b21lcj4gRmlsdGVyKEN1c3RvbWVyQ3JpdGVyaWEgcmVxdWVzdCk7DQogICAgICAgIFRhc2s8Q3VzdG9tZXI+IFN0b3JlKEN1c3RvbWVyIHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Access/Customer/Interface/ICustomerAccess.cs

if [ ! -d Component/Manager/Notification/Interface ];
then
mkdir  Component/Manager/Notification/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTm90aWZpY2F0aW9uLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBpbnRlcmZhY2UgSU5vdGlmaWNhdGlvbk1hbmFnZXINCiAgICB7DQogICAgICAgIFRhc2s8Tm90aWZ5VXNlclJlc3BvbnNlPiBOb3RpZnlVc2VyKE5vdGlmeVVzZXJSZXF1ZXN0IHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Manager/Notification/Interface/INotificationManager.cs

if [ ! -d Component/Utility/Security/Interface ];
then
mkdir  Component/Utility/Security/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLlV0aWxpdHkuU2VjdXJpdHkuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGludGVyZmFjZSBJU2VjdXJpdHlVdGlsaXR5DQogICAgew0KICAgICAgICBUYXNrPEhhc2hSZXNwb25zZT4gSGFzaFBhc3N3b3JkKEhhc2hQYXNzd29yZFJlcXVlc3QgcmVxdWVzdCk7DQogICAgfQ0KfQ==" | base64 -d > Component/Utility/Security/Interface/ISecurityUtility.cs

if [ ! -d Component/Access/Equipment/Interface ];
then
mkdir  Component/Access/Equipment/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCnVzaW5nIEZpeE1lLkFjY2Vzcy5FcXVpcG1lbnQuSW50ZXJmYWNlLkNvbW1vbjsNCg0KbmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5FcXVpcG1lbnQuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGludGVyZmFjZSBJRXF1aXBtZW50QWNjZXNzDQogICAgew0KICAgICAgICBUYXNrPEZpbHRlclJlc3BvbnNlQmFzZT4gRmlsdGVyKEZpbHRlclJlcXVlc3RCYXNlIHJlcXVlc3QpOw0KICAgICAgICBUYXNrPFN0b3JlUmVzcG9uc2VCYXNlPiBTdG9yZShTdG9yZVJlcXVlc3RCYXNlIHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Access/Equipment/Interface/IEquipmentAccess.cs

if [ ! -d Component/Manager/Tasking/Interface ];
then
mkdir  Component/Manager/Tasking/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuVGFza2luZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgaW50ZXJmYWNlIElUYXNraW5nTWFuYWdlcg0KICAgIHsNCiAgICAgICAgVGFzazxDcmVhdGVCYWNrb2ZmaWNlVGFza1Jlc3BvbnNlPiBDcmVhdGVCYWNrb2ZmaWNlVGFzayhDcmVhdGVCYWNrb2ZmaWNlVGFza1JlcXVlc3QgcmVxdWVzdCk7DQogICAgICAgIFRhc2s8Q29uZmlybU1haW50ZW5hbmNlUHJvdmlkZXJTbG90c1Jlc3BvbnNlPiBDb25maXJtTWFpbnRlbmFuY2VQcm92aWRlclNsb3RzKENvbmZpcm1NYWludGVuYW5jZVByb3ZpZGVyU2xvdHNSZXF1ZXN0IHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Manager/Tasking/Interface/ITaskingManager.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGludGVyZmFjZSBJTWFpbnRlbmFuY2VNYW5hZ2VyDQogICAgew0KICAgICAgICBUYXNrPE1hdGNoTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmdSZXNwb25zZT4gTWF0Y2hNYWludGVuYW5jZVBsYW5PZmZlcmluZyhNYXRjaE1haW50ZW5hbmNlUGxhbk9mZmVyaW5nUmVxdWVzdCByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxDcmVhdGVQZW5kaW5nTWFpbnRlbmFuY2VQbGFuUmVzcG9uc2U+IENyZWF0ZVBlbmRpbmdNYWludGVuYW5jZVBsYW4oQ3JlYXRlUGVuZGluZ01haW50ZW5hbmNlUGxhblJlcXVlc3QgcmVxdWVzdCk7DQogICAgICAgIFRhc2s8UmVzb2x2ZVBlbmRpbmdNYWludGVuYW5jZVBsYW5SZXNwb25zZT4gUmVzb2x2ZVBlbmRpbmdNYWludGVuYW5jZVBsYW4oUmVzb2x2ZVBlbmRpbmdNYWludGVuYW5jZVBsYW5SZXF1ZXN0IHJlcXVlc3QpOw0KICAgICAgICBUYXNrPEluaXRpYXRlRVNpZ25pbmdGb3JNYWludGVuYW5jZVBsYW5SZXNwb25zZT4gSW5pdGlhdGVFU2lnbmluZ0Zvck1haW50ZW5hbmNlUGxhbihJbml0aWF0ZUVTaWduaW5nRm9yTWFpbnRlbmFuY2VQbGFuUmVxdWVzdCByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxDcmVhdGVNYWludGVuYW5jZUpvYlNsb3RzUHJvcG9zYWxSZXNwb25zZT4gQ3JlYXRlTWFpbnRlbmFuY2VKb2JTbG90c1Byb3Bvc2FsKENyZWF0ZU1haW50ZW5hbmNlSm9iU2xvdHNQcm9wb3NhbFJlcXVlc3QgcmVxdWVzdCk7DQogICAgICAgIFRhc2s8Q29uZmlybU1haW50ZW5hbmNlUHJvdmlkZXJTbG90c1Jlc3BvbnNlPiBDb25maXJtTWFpbnRlbmFuY2VQcm92aWRlclNsb3RzKENvbmZpcm1NYWludGVuYW5jZVByb3ZpZGVyU2xvdHNSZXF1ZXN0IHJlcXVlc3QpOw0KICAgICAgICBUYXNrPENvbmZpcm1NYWludGVuYW5jZVNsb3RzUHJvcG9zYWxSZXNwb25zZT4gQ29uZmlybU1haW50ZW5hbmNlU2xvdHNQcm9wb3NhbChDb25maXJtTWFpbnRlbmFuY2VTbG90c1Byb3Bvc2FsUmVxdWVzdCByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxBY2NlcHRNYWludGVuYW5jZVNsb3RzUHJvcG9zYWxSZXNwb25zZT4gQWNjZXB0TWFpbnRlbmFuY2VTbG90c1Byb3Bvc2FsKEFjY2VwdE1haW50ZW5hbmNlU2xvdHNQcm9wb3NhbFJlcXVlc3QgcmVxdWVzdCk7DQogICAgICAgIFRhc2s8Q2FuY2VsTWFpbnRlbmFuY2VKb2JSZXNwb25zZT4gQ2FuY2VsTWFpbnRlbmFuY2VKb2IoQ2FuY2VsTWFpbnRlbmFuY2VKb2JSZXF1ZXN0IHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/IMaintenanceManager.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgaW50ZXJmYWNlIElNYWludGVuYW5jZUFjY2Vzcw0KICAgIHsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZVBsYW5PZmZlcmluZz4gTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmdGaWx0ZXIoTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmdDcml0ZXJpYSByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZVBsYW4+IE1haW50ZW5hbmNlUGxhbkZpbHRlcihNYWludGVuYW5jZVBsYW5Dcml0ZXJpYSByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZVBsYW4+IE1haW50ZW5hbmNlUGxhblN0b3JlKE1haW50ZW5hbmNlUGxhbiByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZUpvYlNsb3Q+IE1haW50ZW5hbmNlSm9iU2xvdEZpbHRlcihNYWludGVuYW5jZUpvYlNsb3RDcml0ZXJpYSByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZUpvYlNsb3Q+IE1haW50ZW5hbmNlSm9iU2xvdFN0b3JlKE1haW50ZW5hbmNlSm9iU2xvdCByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZUpvYlNsb3RzUHJvcG9zYWw+IE1haW50ZW5hbmNlSm9iU2xvdHNQcm9wb3NhbFN0b3JlKE1haW50ZW5hbmNlSm9iU2xvdHNQcm9wb3NhbCByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZUpvYlNsb3RzUHJvcG9zYWw+IE1haW50ZW5hbmNlSm9iU2xvdHNQcm9wb3NhbEZpbHRlcihNYWludGVuYW5jZUpvYlNsb3RzUHJvcG9zYWxDcml0ZXJpYSByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZUpvYj4gTWFpbnRlbmFuY2VKb2JTdG9yZShNYWludGVuYW5jZUpvYiByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxNYWludGVuYW5jZUpvYj4gTWFpbnRlbmFuY2VKb2JGaWx0ZXIoTWFpbnRlbmFuY2VKb2JDcml0ZXJpYSByZXF1ZXN0KTsNCiAgICB9DQp9" | base64 -d > Component/Access/Maintenance/Interface/IMaintenanceAccess.cs

if [ ! -d Component/Engine/Matching/Interface ];
then
mkdir  Component/Engine/Matching/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5NYXRjaGluZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgaW50ZXJmYWNlIElNYXRjaGluZ0VuZ2luZQ0KICAgIHsNCiAgICAgICAgVGFzazxNYXRjaE1haW50ZW5hbmNlUGxhbk9mZmVyaW5nUmVzcG9uc2U+IE1hdGNoTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmcoTWF0Y2hNYWludGVuYW5jZVBsYW5PZmZlcmluZ1JlcXVlc3QgcmVxdWVzdCk7DQogICAgfQ0KfQ==" | base64 -d > Component/Engine/Matching/Interface/IMatchingEngine.cs

if [ ! -d Component/Access/Task/Interface ];
then
mkdir  Component/Access/Task/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5UYXNrLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBpbnRlcmZhY2UgSVRhc2tBY2Nlc3MNCiAgICB7DQogICAgICAgIFRhc2s8VGFzaz4gRmlsdGVyKFRhc2tDcml0ZXJpYSByZXF1ZXN0KTsNCiAgICAgICAgVGFzazxUYXNrPiBTdG9yZShUYXNrIHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Access/Task/Interface/ITaskAccess.cs

if [ ! -d Component/Access/Agreement/Interface ];
then
mkdir  Component/Access/Agreement/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5BZ3JlZW1lbnQuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGludGVyZmFjZSBJQWdyZWVtZW50QWNjZXNzDQogICAgew0KICAgICAgICBUYXNrPEFncmVlbWVudD4gRmlsdGVyKEFncmVlbWVudENyaXRlcmlhIHJlcXVlc3QpOw0KICAgICAgICBUYXNrPEFncmVlbWVudD4gU3RvcmUoQWdyZWVtZW50IHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Access/Agreement/Interface/IAgreementAccess.cs

if [ ! -d Component/Access/ESigning/Interface ];
then
mkdir  Component/Access/ESigning/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5FU2lnbmluZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgaW50ZXJmYWNlIElFU2lnbmluZ0FjY2Vzcw0KICAgIHsNCiAgICAgICAgVGFzazxTaWduYXR1cmVSZXF1ZXN0PiBTaWduYXR1cmVSZXF1ZXN0U3RvcmUoU2lnbmF0dXJlUmVxdWVzdCByZXF1ZXN0KTsNCiAgICB9DQp9" | base64 -d > Component/Access/ESigning/Interface/IESigningAccess.cs

if [ ! -d Component/Engine/Scheduling/Interface ];
then
mkdir  Component/Engine/Scheduling/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5TY2hlZHVsaW5nLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBpbnRlcmZhY2UgSVNjaGVkdWxpbmdFbmdpbmUNCiAgICB7DQogICAgICAgIFRhc2s8Q3JlYXRlTWFpbnRlbmFuY2VKb2JTbG90c1Byb3Bvc2FsUmVzcG9uc2U+IENyZWF0ZU1haW50ZW5hbmNlSm9iU2xvdHNQcm9wb3NhbChDcmVhdGVNYWludGVuYW5jZUpvYlNsb3RzUHJvcG9zYWxSZXF1ZXN0IHJlcXVlc3QpOw0KICAgICAgICBUYXNrPEFjY2VwdE1haW50ZW5hbmNlU2xvdHNQcm9wb3NhbFJlc3BvbnNlPiBBY2NlcHRNYWludGVuYW5jZVNsb3RzUHJvcG9zYWwoQWNjZXB0TWFpbnRlbmFuY2VTbG90c1Byb3Bvc2FsUmVxdWVzdCByZXF1ZXN0KTsNCiAgICB9DQp9" | base64 -d > Component/Engine/Scheduling/Interface/ISchedulingEngine.cs

if [ ! -d Component/Access/Notification/Interface ];
then
mkdir  Component/Access/Notification/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5Ob3RpZmljYXRpb24uSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGludGVyZmFjZSBJTm90aWZpY2F0aW9uQWNjZXNzDQogICAgew0KICAgICAgICBUYXNrPE5vdGlmaWNhdGlvblRlbXBsYXRlPiBOb3RpZmljYXRpb25UZW1wbGF0ZUZpbHRlcihOb3RpZmljYXRpb25UZW1wbGF0ZUNyaXRlcmlhIHJlcXVlc3QpOw0KICAgICAgICBUYXNrPE5vdGlmaWNhdGlvbj4gU3RvcmUoTm90aWZpY2F0aW9uIHJlcXVlc3QpOw0KICAgIH0NCn0=" | base64 -d > Component/Access/Notification/Interface/INotificationAccess.cs

if [ ! -d Component/Utility/Communication/Interface ];
then
mkdir  Component/Utility/Communication/Interface
fi
echo "dXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsNCg0KbmFtZXNwYWNlIEZpeE1lLlV0aWxpdHkuQ29tbXVuaWNhdGlvbi5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgaW50ZXJmYWNlIElDb21tdW5pY2F0aW9uVXRpbGl0eQ0KICAgIHsNCiAgICAgICAgVGFzazxOb3RpZnlVc2VyUmVzcG9uc2U+IE5vdGlmeVVzZXIoTm90aWZ5VXNlclJlcXVlc3QgcmVxdWVzdCk7DQogICAgfQ0KfQ==" | base64 -d > Component/Utility/Communication/Interface/ICommunicationUtility.cs

if [ ! -d Component/Access/Equipment/Interface/Common ];
then
mkdir  Component/Access/Equipment/Interface/Common
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5FcXVpcG1lbnQuSW50ZXJmYWNlLkNvbW1vbg0Kew0KICAgIHB1YmxpYyBhYnN0cmFjdCBjbGFzcyBGaWx0ZXJSZXF1ZXN0QmFzZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Equipment/Interface/Common/FilterRequestBase.cs

if [ ! -d Component/Access/Equipment/Interface/Common ];
then
mkdir  Component/Access/Equipment/Interface/Common
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5FcXVpcG1lbnQuSW50ZXJmYWNlLkNvbW1vbg0Kew0KICAgIHB1YmxpYyBhYnN0cmFjdCBjbGFzcyBTdG9yZVJlcXVlc3RCYXNlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Access/Equipment/Interface/Common/StoreRequestBase.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgUmVnaXN0ZXJBY2NvdW50UmVxdWVzdA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Membership/Interface/RegisterAccountRequest.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgUmVnaXN0ZXJBY2NvdW50UmVzcG9uc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Membership/Interface/RegisterAccountResponse.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ29uZmlybVVzZXJFbWFpbFJlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Membership/Interface/ConfirmUserEmailRequest.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ29uZmlybVVzZXJFbWFpbFJlc3BvbnNlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Membership/Interface/ConfirmUserEmailResponse.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgVXBkYXRlVXNlclBhc3N3b3JkUmVxdWVzdA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Membership/Interface/UpdateUserPasswordRequest.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgVXBkYXRlVXNlclBhc3N3b3JkUmVzcG9uc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Membership/Interface/UpdateUserPasswordResponse.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgU2V0VXNlclByZWZlcmVuY2VzUmVxdWVzdA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Membership/Interface/SetUserPreferencesRequest.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgU2V0VXNlclByZWZlcmVuY2VzUmVzcG9uc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Membership/Interface/SetUserPreferencesResponse.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ3JlYXRlUGVuZGluZ1JlZ2lzdHJhdGlvblJlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Membership/Interface/CreatePendingRegistrationRequest.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgUGVuZGluZ1JlZ2lzdHJhdGlvbg0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Membership/Interface/PendingRegistration.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgUmVzb2x2ZVBlbmRpbmdSZWdpc3RyYXRpb25SZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Membership/Interface/ResolvePendingRegistrationRequest.cs

if [ ! -d Component/Manager/Membership/Interface ];
then
mkdir  Component/Manager/Membership/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWVtYmVyc2hpcC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgUmVzb2x2ZVBlbmRpbmdSZWdpc3RyYXRpb25SZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Membership/Interface/ResolvePendingRegistrationResponse.cs

if [ ! -d Component/Access/Customer/Interface ];
then
mkdir  Component/Access/Customer/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5DdXN0b21lci5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ3VzdG9tZXJDcml0ZXJpYQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Customer/Interface/CustomerCriteria.cs

if [ ! -d Component/Access/Customer/Interface ];
then
mkdir  Component/Access/Customer/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5DdXN0b21lci5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ3VzdG9tZXINCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Customer/Interface/Customer.cs

if [ ! -d Component/Manager/Notification/Interface ];
then
mkdir  Component/Manager/Notification/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTm90aWZpY2F0aW9uLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBOb3RpZnlVc2VyUmVxdWVzdA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Notification/Interface/NotifyUserRequest.cs

if [ ! -d Component/Manager/Notification/Interface ];
then
mkdir  Component/Manager/Notification/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTm90aWZpY2F0aW9uLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBOb3RpZnlVc2VyUmVzcG9uc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Notification/Interface/NotifyUserResponse.cs

if [ ! -d Component/Utility/Security/Interface ];
then
mkdir  Component/Utility/Security/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLlV0aWxpdHkuU2VjdXJpdHkuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEhhc2hQYXNzd29yZFJlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Utility/Security/Interface/HashPasswordRequest.cs

if [ ! -d Component/Utility/Security/Interface ];
then
mkdir  Component/Utility/Security/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLlV0aWxpdHkuU2VjdXJpdHkuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEhhc2hSZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Utility/Security/Interface/HashResponse.cs

if [ ! -d Component/Manager/Tasking/Interface ];
then
mkdir  Component/Manager/Tasking/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuVGFza2luZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ3JlYXRlQmFja29mZmljZVRhc2tSZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Tasking/Interface/CreateBackofficeTaskResponse.cs

if [ ! -d Component/Manager/Tasking/Interface ];
then
mkdir  Component/Manager/Tasking/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuVGFza2luZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ3JlYXRlQmFja29mZmljZVRhc2tSZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Tasking/Interface/CreateBackofficeTaskRequest.cs

if [ ! -d Component/Access/Equipment/Interface ];
then
mkdir  Component/Access/Equipment/Interface
fi
echo "dXNpbmcgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UuQ29tbW9uOw0KDQpuYW1lc3BhY2UgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgRXF1aXBtZW50VHlwZUNyaXRlcmlhIDogRmlsdGVyUmVxdWVzdEJhc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Equipment/Interface/EquipmentTypeCriteria.cs

if [ ! -d Component/Access/Equipment/Interface ];
then
mkdir  Component/Access/Equipment/Interface
fi
echo "dXNpbmcgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UuQ29tbW9uOw0KDQpuYW1lc3BhY2UgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgRXF1aXBtZW50VHlwZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Equipment/Interface/EquipmentType.cs

if [ ! -d Component/Access/Equipment/Interface ];
then
mkdir  Component/Access/Equipment/Interface
fi
echo "dXNpbmcgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UuQ29tbW9uOw0KDQpuYW1lc3BhY2UgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgUGVuZGluZ1JlZ2lzdHJhdGlvbiA6IFN0b3JlUmVxdWVzdEJhc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Equipment/Interface/PendingRegistration.cs

if [ ! -d Component/Access/Equipment/Interface ];
then
mkdir  Component/Access/Equipment/Interface
fi
echo "dXNpbmcgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UuQ29tbW9uOw0KDQpuYW1lc3BhY2UgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgUGVuZGluZ1JlZ2lzdHJhdGlvbkNyaXRlcmlhIDogRmlsdGVyUmVxdWVzdEJhc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Equipment/Interface/PendingRegistrationCriteria.cs

if [ ! -d Component/Access/Equipment/Interface ];
then
mkdir  Component/Access/Equipment/Interface
fi
echo "dXNpbmcgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UuQ29tbW9uOw0KDQpuYW1lc3BhY2UgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgRXF1aXBtZW50IDogU3RvcmVSZXF1ZXN0QmFzZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Equipment/Interface/Equipment.cs

if [ ! -d Component/Access/Equipment/Interface ];
then
mkdir  Component/Access/Equipment/Interface
fi
echo "dXNpbmcgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UuQ29tbW9uOw0KDQpuYW1lc3BhY2UgRml4TWUuQWNjZXNzLkVxdWlwbWVudC5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgRXF1aXBtZW50Q3JpdGVyaWEgOiBGaWx0ZXJSZXF1ZXN0QmFzZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Equipment/Interface/EquipmentCriteria.cs

if [ ! -d Component/Manager/Tasking/Interface ];
then
mkdir  Component/Manager/Tasking/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuVGFza2luZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ29uZmlybU1haW50ZW5hbmNlUHJvdmlkZXJTbG90c1JlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Tasking/Interface/ConfirmMaintenanceProviderSlotsRequest.cs

if [ ! -d Component/Manager/Tasking/Interface ];
then
mkdir  Component/Manager/Tasking/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuVGFza2luZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgQ29uZmlybU1haW50ZW5hbmNlUHJvdmlkZXJTbG90c1Jlc3BvbnNlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Tasking/Interface/ConfirmMaintenanceProviderSlotsResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIE1hdGNoTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmdSZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/MatchMaintenancePlanOfferingRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIE1hdGNoTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmdSZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Maintenance/Interface/MatchMaintenancePlanOfferingResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENyZWF0ZVBlbmRpbmdNYWludGVuYW5jZVBsYW5SZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/CreatePendingMaintenancePlanRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENyZWF0ZVBlbmRpbmdNYWludGVuYW5jZVBsYW5SZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Maintenance/Interface/CreatePendingMaintenancePlanResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIFJlc29sdmVQZW5kaW5nTWFpbnRlbmFuY2VQbGFuUmVxdWVzdA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Maintenance/Interface/ResolvePendingMaintenancePlanRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIFJlc29sdmVQZW5kaW5nTWFpbnRlbmFuY2VQbGFuUmVzcG9uc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Maintenance/Interface/ResolvePendingMaintenancePlanResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEluaXRpYXRlRVNpZ25pbmdGb3JNYWludGVuYW5jZVBsYW5SZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/InitiateESigningForMaintenancePlanRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEluaXRpYXRlRVNpZ25pbmdGb3JNYWludGVuYW5jZVBsYW5SZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Maintenance/Interface/InitiateESigningForMaintenancePlanResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENyZWF0ZU1haW50ZW5hbmNlSm9iU2xvdHNQcm9wb3NhbFJlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Maintenance/Interface/CreateMaintenanceJobSlotsProposalRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENyZWF0ZU1haW50ZW5hbmNlSm9iU2xvdHNQcm9wb3NhbFJlc3BvbnNlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/CreateMaintenanceJobSlotsProposalResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENvbmZpcm1NYWludGVuYW5jZVByb3ZpZGVyU2xvdHNSZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/ConfirmMaintenanceProviderSlotsRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENvbmZpcm1NYWludGVuYW5jZVByb3ZpZGVyU2xvdHNSZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Maintenance/Interface/ConfirmMaintenanceProviderSlotsResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENvbmZpcm1NYWludGVuYW5jZVNsb3RzUHJvcG9zYWxSZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/ConfirmMaintenanceSlotsProposalRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENvbmZpcm1NYWludGVuYW5jZVNsb3RzUHJvcG9zYWxSZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Maintenance/Interface/ConfirmMaintenanceSlotsProposalResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEFjY2VwdE1haW50ZW5hbmNlU2xvdHNQcm9wb3NhbFJlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Maintenance/Interface/AcceptMaintenanceSlotsProposalRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEFjY2VwdE1haW50ZW5hbmNlU2xvdHNQcm9wb3NhbFJlc3BvbnNlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Manager/Maintenance/Interface/AcceptMaintenanceSlotsProposalResponse.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENhbmNlbE1haW50ZW5hbmNlSm9iUmVxdWVzdA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Manager/Maintenance/Interface/CancelMaintenanceJobRequest.cs

if [ ! -d Component/Manager/Maintenance/Interface ];
then
mkdir  Component/Manager/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLk1hbmFnZXIuTWFpbnRlbmFuY2UuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIENhbmNlbE1haW50ZW5hbmNlSm9iUmVzcG9uc2UNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Manager/Maintenance/Interface/CancelMaintenanceJobResponse.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmdDcml0ZXJpYQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Maintenance/Interface/MaintenancePlanOfferingCriteria.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VQbGFuT2ZmZXJpbmcNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Maintenance/Interface/MaintenancePlanOffering.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VQbGFuQ3JpdGVyaWENCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Maintenance/Interface/MaintenancePlanCriteria.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VQbGFuDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Access/Maintenance/Interface/MaintenancePlan.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VKb2JTbG90Q3JpdGVyaWENCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Maintenance/Interface/MaintenanceJobSlotCriteria.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VKb2JTbG90DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Access/Maintenance/Interface/MaintenanceJobSlot.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VKb2JTbG90c1Byb3Bvc2FsDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Access/Maintenance/Interface/MaintenanceJobSlotsProposal.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VKb2JTbG90c1Byb3Bvc2FsQ3JpdGVyaWENCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Maintenance/Interface/MaintenanceJobSlotsProposalCriteria.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VKb2INCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Maintenance/Interface/MaintenanceJob.cs

if [ ! -d Component/Access/Maintenance/Interface ];
then
mkdir  Component/Access/Maintenance/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5NYWludGVuYW5jZS5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWFpbnRlbmFuY2VKb2JDcml0ZXJpYQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Maintenance/Interface/MaintenanceJobCriteria.cs

if [ ! -d Component/Engine/Matching/Interface ];
then
mkdir  Component/Engine/Matching/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5NYXRjaGluZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWF0Y2hNYWludGVuYW5jZVBsYW5PZmZlcmluZ1JlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Engine/Matching/Interface/MatchMaintenancePlanOfferingRequest.cs

if [ ! -d Component/Engine/Matching/Interface ];
then
mkdir  Component/Engine/Matching/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5NYXRjaGluZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTWF0Y2hNYWludGVuYW5jZVBsYW5PZmZlcmluZ1Jlc3BvbnNlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Engine/Matching/Interface/MatchMaintenancePlanOfferingResponse.cs

if [ ! -d Component/Access/Task/Interface ];
then
mkdir  Component/Access/Task/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5UYXNrLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBUYXNrQ3JpdGVyaWENCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Task/Interface/TaskCriteria.cs

if [ ! -d Component/Access/Task/Interface ];
then
mkdir  Component/Access/Task/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5UYXNrLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBUYXNrDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Access/Task/Interface/Task.cs

if [ ! -d Component/Access/Agreement/Interface ];
then
mkdir  Component/Access/Agreement/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5BZ3JlZW1lbnQuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEFncmVlbWVudENyaXRlcmlhDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Access/Agreement/Interface/AgreementCriteria.cs

if [ ! -d Component/Access/Agreement/Interface ];
then
mkdir  Component/Access/Agreement/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5BZ3JlZW1lbnQuSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIEFncmVlbWVudA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Agreement/Interface/Agreement.cs

if [ ! -d Component/Access/ESigning/Interface ];
then
mkdir  Component/Access/ESigning/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5FU2lnbmluZy5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgU2lnbmF0dXJlUmVxdWVzdA0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/ESigning/Interface/SignatureRequest.cs

if [ ! -d Component/Engine/Scheduling/Interface ];
then
mkdir  Component/Engine/Scheduling/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5TY2hlZHVsaW5nLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBDcmVhdGVNYWludGVuYW5jZUpvYlNsb3RzUHJvcG9zYWxSZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Engine/Scheduling/Interface/CreateMaintenanceJobSlotsProposalRequest.cs

if [ ! -d Component/Engine/Scheduling/Interface ];
then
mkdir  Component/Engine/Scheduling/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5TY2hlZHVsaW5nLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBDcmVhdGVNYWludGVuYW5jZUpvYlNsb3RzUHJvcG9zYWxSZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Engine/Scheduling/Interface/CreateMaintenanceJobSlotsProposalResponse.cs

if [ ! -d Component/Engine/Scheduling/Interface ];
then
mkdir  Component/Engine/Scheduling/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5TY2hlZHVsaW5nLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBBY2NlcHRNYWludGVuYW5jZVNsb3RzUHJvcG9zYWxSZXF1ZXN0DQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Engine/Scheduling/Interface/AcceptMaintenanceSlotsProposalRequest.cs

if [ ! -d Component/Engine/Scheduling/Interface ];
then
mkdir  Component/Engine/Scheduling/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkVuZ2luZS5TY2hlZHVsaW5nLkludGVyZmFjZQ0Kew0KICAgIHB1YmxpYyBjbGFzcyBBY2NlcHRNYWludGVuYW5jZVNsb3RzUHJvcG9zYWxSZXNwb25zZQ0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Engine/Scheduling/Interface/AcceptMaintenanceSlotsProposalResponse.cs

if [ ! -d Component/Access/Notification/Interface ];
then
mkdir  Component/Access/Notification/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5Ob3RpZmljYXRpb24uSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIE5vdGlmaWNhdGlvblRlbXBsYXRlQ3JpdGVyaWENCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Access/Notification/Interface/NotificationTemplateCriteria.cs

if [ ! -d Component/Access/Notification/Interface ];
then
mkdir  Component/Access/Notification/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5Ob3RpZmljYXRpb24uSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIE5vdGlmaWNhdGlvblRlbXBsYXRlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Access/Notification/Interface/NotificationTemplate.cs

if [ ! -d Component/Access/Notification/Interface ];
then
mkdir  Component/Access/Notification/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLkFjY2Vzcy5Ob3RpZmljYXRpb24uSW50ZXJmYWNlDQp7DQogICAgcHVibGljIGNsYXNzIE5vdGlmaWNhdGlvbg0KICAgIHsNCiAgICB9DQp9" | base64 -d > Component/Access/Notification/Interface/Notification.cs

if [ ! -d Component/Utility/Communication/Interface ];
then
mkdir  Component/Utility/Communication/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLlV0aWxpdHkuQ29tbXVuaWNhdGlvbi5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTm90aWZ5VXNlclJlcXVlc3QNCiAgICB7DQogICAgfQ0KfQ==" | base64 -d > Component/Utility/Communication/Interface/NotifyUserRequest.cs

if [ ! -d Component/Utility/Communication/Interface ];
then
mkdir  Component/Utility/Communication/Interface
fi
echo "bmFtZXNwYWNlIEZpeE1lLlV0aWxpdHkuQ29tbXVuaWNhdGlvbi5JbnRlcmZhY2UNCnsNCiAgICBwdWJsaWMgY2xhc3MgTm90aWZ5VXNlclJlc3BvbnNlDQogICAgew0KICAgIH0NCn0=" | base64 -d > Component/Utility/Communication/Interface/NotifyUserResponse.cs
