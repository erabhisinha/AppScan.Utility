# AppScan.Utility
To execute this utility we have to pass three parameters:
1 - Regex expression to identify the kwywords
2 - Source code repo path
3 - Output reporzts path


Ex:

AppScan.Utility.exe "(pwd(\s)*(=){1}(\s)*(\""|\').*(\""|\'))|(password(\s)*(=){1}(\s)*(\""|\').*(\""|\'))" "D:\UCM\Staging30\Web" "D:\RnD"
