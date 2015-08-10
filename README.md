<h1>AzureMetricSample</h1>
<p>This sample shows how can you read VM metrics using Azure Insights REST API. <br>
To use this API metric you always needs to do two calls: first to retrieve metric definition and the second to read the metrics values.
</p>

<h2>Pre-requisites</h2>
<ol>
<li>Have a VM created in Azure with diagnostic activated</li>
<li>Have a user created in Azure AD</li>
<li>Grant this user as a reader of the VM</li><a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/users.JPG">
<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/users.JPG" height="40%" width="40%">
</a>

<li>Create an application in Azure AD and gives permission to other application <b>Windows Azure Service Manager API</b></li>
<a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/permissions.JPG">
<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/permissions.JPG" height="40%" width="40%">
</a>
</ol>


<h2>Configure the sample code</h2>
You need to configure APP.config with this data
<ol>
<li>tenantId: you can see your tenant id in Active directory, application endpoints <br>
<a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/tenatId.JPG">
<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/tenatId.JPG" height="40%" width="40%"></A>
</li>
<li>clientId:You can read client id in e Active directory, application property<br>
<a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/clientID.JPG">
<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/clientID.JPG" height="40%" width="40%"></A>
</li>
<li>userName: username from Azure Active Directory</li>
<li>password: user password</li>
<li>SubscriptionId:  subscription ID from the resource, in this case the VM</li>
</ol>

<p>On the code you need to change the resource with your data
<pre>
  string ResourceGroupName = "metricsampleRG";
  string ProviderName="Microsoft.Compute";
  string VirtualMachineName="metricsample";
</pre>
This data can ead from Resource ID in the new portal<br>
<a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/resourceID.JPG">
<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/resourceID.JPG" height="40%" width="40%"></A>
<br>
and looks like this sample <br>
/subscriptions/d134b748-cba2-43e2-a445-edc634076368/resourceGroups/<b>metricsampleRG</b>/providers/<b>Microsoft.Compute</b>/virtualMachines/<b>metricsample</b>
</p>

<h2>Run</h2>
<p>When run the sample program you will see:<br>
	<ol>
		<li>The Token<br>
		<a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/token.JPG">
		<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/token.JPG" height="40%" width="40%"></A>
		</li>
		<li>
			The metric list for this recource<br>
			<a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/metricList.JPG">
			<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/metricList.JPG" height="40%" width="40%"></A>
		</li>
		<li>
			The metric values for this recource(filtered)<br>
			<a href="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/values.JPG">
			<img src="https://raw.githubusercontent.com/liarjo/AzureMetricSample/master/img/values.JPG" height="40%" width="40%"></A>
		</li>
	</ol>	
</p>

