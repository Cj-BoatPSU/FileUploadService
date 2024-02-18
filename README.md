# File Upload Service
<p> เป็น Service ที่ทำหน้าที่ Upload File แล้วเก็บไฟล์ไว้ที่ Server สามารถ Upload File, Get Info File, Dowload File และ Delete File ได้ </p>

![](/Pic/8.PNG)

<p> ก่อนจะสามารถใช้งาน Service ได้ต้องทำการ Authentication ก่อน ผ่านการยิง Postman เพื่อให้ได้ Token มา เพราะ API แต่เส้นมีการ Authorize ทุกเส้น </p>

![](/Pic/1.PNG)

# UploadFile API

![](/Pic/2.PNG)

## Send Mail
<p> หลังจากที่ Upload File ก็จะมีการแจ้งเตือนโดยการส่ง Mail </p>

![](/Pic/9.PNG)

## Upload Folder
<p> หลังจากที่ Upload File จะถูกเก็บไว้ใน Server อยู่ใน Upload Folder ดังภาพ </p>

![](/Pic/10.png)

# GetInfo API

![](/Pic/3.PNG)

# DownloadFile API

![](/Pic/4.PNG)

# DeleteFile API

![](/Pic/5.PNG)

# File Table

![](/Pic/6.PNG)

# Account Table

![](/Pic/7.PNG)
