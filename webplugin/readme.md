Web Plugin for generating an email with attachment(s)

Installation:

1) Register assembly

- Copy file "Ctse.SO.AjaxTest.dll" to bin-folder
- In file "web.config" add the assembly to the section: 

<pre>
    &lt;Factory&gt;
      &lt;DynamicLoad&gt;
        ....
	      &lt;add key=&quot;Ctse&quot; value=&quot;Ctse.SO.AjaxTest.dll&quot; /&gt;
      &lt;/DynamicLoad&gt;
    &lt;/Factory&gt;
</pre>

2) Add JavaScript

- In folder \JavaScripts\ create subfolder "Ctse" and place file "ctse.so.ajaxtest.js"
- In file "\App_Data\WebClient\Web\SystemFiles\SoApplicationConfiguration.config" add the script to the section:

<pre>
  &lt;jsincludes&gt;
    ...
    &lt;jsinclude path=&quot;~/JavaScripts/Ctse/ctse.so.ajaxtest.js&quot; /&gt;
  &lt;/jsincludes&gt;
</pre>

3) Add button in webapplication
...
  
