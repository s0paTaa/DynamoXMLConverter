# DynamoXMLConverter
<span>
  Welcome to Dynamo XML to JSON converter. This is a simple web app that allows you to convert one or multiple XML files with ability to preview and download them as JSON.
Application has three pages to interact with the user. On the main (Home) page are places several things like information badges, navigation buttons to the other pages, simple form for uploading the XML file and last but not least,
you have section with listed JSON converted items from which you can navigate directly to preview page or download the file. Other two pages are simple, the preview page have information badge, home button, simple form for entering the file identifier,
which is needed to find the existing file and preview section desinged a little bit smoothly. Last page (Download) have almost the same thing as preview page but without the JSON preview section.
</span>
<h3>There are several validations that you need to know before upload XML file.</h3>
<ul>
  <li>Size limit per file is <strong>4 MB</strong></li>
  <li>Maximum size limit of all uploaded files is <strong>256 MB</strong></li>
  <li>Form accepts only <strong>XML</strong> files</li>
  <li>Files with some ransome malware will be repelled</li>
</ul>
<h3>Functionalities</h3>
<ul>
  <li>File validation for size, type, virus and extension</li>
  <li>SQL database storage for all uploaded files until they expire. <strong>Expiration time is 10 days</strong></li>
  <li>ClamAv antimalware detector. <strong>Require installation and configuration</strong></li>
  <li>Hangfire job, executed on every hour to check for expired files and to remove them from database</li>
  <li>File conversion from XML to JSON using Newtonsoft.Json</li>
  <li>Using strong middleware to prevent XSS attacks</li>
  <li>Global exception handler</li>
  <li>Redirection from the pages</li>
</ul>

# Installastion
<h3>1. Install and configure ClamAV. Without it, stored files are not completely safe.</h1>
<h6>You can find the latest Windows package of ClamAV in the "Alternate Versions of ClamAV" section <a href="https://www.clamav.net/downloads">here</a>.</h6>
