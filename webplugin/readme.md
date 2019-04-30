## SOWeb Plugin for generating an email (eml-file) with attachment(s)

Note: the project uses an external library (Lumisoft.Net), ILMerge will merge this library with the project library when building the project in Visual Studio.

See also:
https://community.superoffice.com/documentation/SDK/SO.Web.Application/html/CustomAjaxMethods.htm

#### Installation plugin in SOWeb:

1) Go to the installationfolder of SoWeb (C:\Program Files\SuperOffice\SOWeb8\)

2) Register assembly:

- Copy file "Ctse.SO.AjaxTest.dll" to the subfolder \bin\
- Open the file "web.config", change:

<pre>
    &lt;Factory&gt;
      &lt;DynamicLoad&gt;
        ....
	&lt;add key=&quot;Ctse&quot; value=&quot;Ctse.SO.AjaxTest.dll&quot; /&gt;
      &lt;/DynamicLoad&gt;
    &lt;/Factory&gt;
</pre>

3) Add JavaScript:

- Go to subfolder \JavaScripts\ and create subfolder "Ctse" and place file "ctse.so.ajaxtest.js"
- Go to subfolder \App_Data\WebClient\Web\SystemFiles\ and open the file SoApplicationConfiguration.config" 
- Change:

<pre>
  &lt;jsincludes&gt;
    ...
    &lt;jsinclude path=&quot;~/JavaScripts/Ctse/ctse.so.ajaxtest.js&quot; /&gt;
  &lt;/jsincludes&gt;
</pre>

4) Add button or menuitem in SOWeb

Add a button/menuitem from where the action (generating the email) should be fired.
For example take a look in the file \App_Data\WebClient\Web\Menu\SoMenuConfiguration.config
In the button/menuitem definition make the call to the JavaScript (note that all files - mailtemplate and attachments - should be in the \so_arc\template\ folder on the server):
<pre>
&lt;url&gt;javascript:GenerateEmailWithAttachments(mailTemplateName, contactId, personId, projectId, saleId, attachmentFileNames);&lt;/url&gt;
</pre>
for example (set the ids on 0 if they are not applicable, attachmentFileNames can also be an empty string if no attachments should be added):
<pre>
GenerateEmailWithAttachments('test1.somail', 1, 2, 0, 0, 'attachment1.pdf,attachment2.pdf,attachment3.pdf')
</pre>

5) Restart the IIS


