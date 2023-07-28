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
<h3>1: Install and configure ClamAV. Without it, stored files are not completely safe.</h1>
<h5>You can find the latest Windows package of ClamAV in the "Alternate Versions of ClamAV" section <a href="https://www.clamav.net/downloads">here</a>.</h5>
<h3>2: Installing and Configuration Files </h3>
<h5>After you download and extract (or install) ClamAV, navigate to the install directory and look for the <strong>conf_examples</strong> folder. In here, there are two files; <strong>clamd.conf.sample</strong> and <strong>freshclam.conf.sample</strong>. Copy these files to the installation directory.</h5>
<h3>3: Modifying Config Files</h3>
<h5>3.1: Modifying clamd.conf.sample:
Open clamd.conf.sample with a text editor, such as notepad.
In Line number 8, you should see the word Example. Comment it so that it looks like: #Example or remove this line.
To enable logging, remove the # in #LogFile "C:\Program Files\ClamAV\clamd.log"
Search for "TCPSocket" and confirm the port is 3310 and it is NOT commented out.
Additionally, confirm "TCPAddr" is NOT commented out. You can either leave it at localhost or specify the ip addresses where ClamAV is installed at.
Save the file and exit. Rename the clamd.conf.sample to clamd.conf</h5>
<h5>3.2: Modifying freshclam.conf.sample
Open freshclam.conf.sample with a text editor, such as notepad.
Similar to clamd.conf, in Line number 8, you should see the word Example. Comment it so that it looks like: #Example or remove this line.
To enable logging, remove the # in the line #UpdateLogFile "C:\Program Files\ClamAV\freshclam.log"
Save the file and exit. Rename freshclam.conf.sample to freshclam.conf</h5>
<h3>4: Install Service and update Antivirus Library</h3>
<h5>Open an elevated Windows DOS (Command Prompt in Administrator Mode) and navigate to the install directory.
Run the following command: clamd.exe --install // This will install the clamd service. 
Run the following code: freshclam.exe // This will update the current library. You can schedule this process via Windows Task Scheduler if you want to frequently update the library.</h5>
<h3>5: Running the Service</h3>
<h5>Open your Windows Services window and start the service: ClamAV ClamD
If you wish, you can also edit the service to launch at startup now.</h5>
<h3>Install Visual Studio 2022</h3>
<h3>Install SQL Server</h3>

# Instalation Resources
<h3>Here are some good resources about installation for ClamAV</h3>
<h5><a href="https://github.com/tekmaven/nClam">Github repo</a></h5>
<h5><a href="https://www.xeams.com/clamav-windows.htm">Instalation tutorial</a></h5>

