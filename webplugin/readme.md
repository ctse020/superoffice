SOWeb Plugin for generating an email (eml-file) with attachment(s)

Note: the project uses an external library (Lumisoft.Net), ILMerge will merge this library with the project library when building the project in Visual Studio.

Installation plugin in SOWeb:

1) Go to the installationfolder of SoWeb (C:\Program Files\SuperOffice\SOWeb8\)

2) Register assembly

- Copy file "Ctse.SO.AjaxTest.dll" to the subfolder \bin\
- In file "web.config" add the assembly to the section: 

<pre>
    &lt;Factory&gt;
      &lt;DynamicLoad&gt;
        ....
	      &lt;add key=&quot;Ctse&quot; value=&quot;Ctse.SO.AjaxTest.dll&quot; /&gt;
      &lt;/DynamicLoad&gt;
    &lt;/Factory&gt;
</pre>

3) Add JavaScript

- Go to subfolder \JavaScripts\ and create subfolder "Ctse" and place file "ctse.so.ajaxtest.js"
- Go to subfolder \App_Data\WebClient\Web\SystemFiles\ and open the file SoApplicationConfiguration.config" 
- add the script to the section:

<pre>
  &lt;jsincludes&gt;
    ...
    &lt;jsinclude path=&quot;~/JavaScripts/Ctse/ctse.so.ajaxtest.js&quot; /&gt;
  &lt;/jsincludes&gt;
</pre>

4) Add button or menuitem in SOWeb

<pre>
...
</pre>

5) Restart the IIS
