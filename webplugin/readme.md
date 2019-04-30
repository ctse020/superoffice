Web Plugin for generating an email with attachment(s)

Installation:

1) Register assembly

- Copy file "Ctse.SO.AjaxTest.dll" to bin-folder
- In file "web.config" add the assembly to the section: 
    <Factory>
      <DynamicLoad>
        ....
	      <add key="Ctse" value="Ctse.SO.AjaxTest.dll" />
      </DynamicLoad>
    </Factory>

2) Add JavaScript

- In folder \JavaScripts\ create subfolder "Ctse" and place file "ctse.so.ajaxtest.js"
- In file "\App_Data\WebClient\Web\SystemFiles\SoApplicationConfiguration.config" add:
  <jsincludes>
    ...
    <jsinclude path="~/JavaScripts/Ctse/ctse.so.ajaxtest.js" />
  </jsincludes>

3) Add button in webapplication
...
  
