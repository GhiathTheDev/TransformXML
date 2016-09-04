# TransformXML Tool
## 1. Description
TransformXML is a tool that can be used to apply XSLT transformations on multiple XML files. The tool do the transformation in two phases:
 * The Cleanup phase: making sure that the XML definition tag is the first tag in the file.
 * The Transform phase: applying XSLT transformation. The XSLT file provided with the tool will add the indentation and line-breakes to the XML, removes the comments and remove unnecessary whitespaces to the right and left of the entity values. One can replace the XSLT transformation template and customize to suite their needs.

## 2. Parameters
 * **`[files Path]`**: Path to where the files to be transformed are located. 
 * **`[XSLT file path]`**: The XSLT file path and name.

## 3. Call Example
* `TransformXML.exe "C:\TEMP\" "XSLT/XSLTIndent.xslt"`
* `TransformXML.exe -help`

---
<span style="color:blue; font-size:12px;">&copy; 2016 [Ghiath Al-Qaisi]</span>


[Ghiath Al-Qaisi]: mailto:ghiath.alqaisi@gmail.com "ghiath.alqaisi@gmail.com"