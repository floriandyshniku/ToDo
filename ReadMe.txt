

//   Firstly set upp the proper connection string into appsetting.json file
   for example : 

   "Data Source=DESKTOP-KKSH6NU\\SQLSERVER2022;Database=ToDo;User=sa;Password=********;
   Encrypt=True;Trusted_Connection=True;Trust Server Certificate=True"

// secondly: Add-Migration [name], Update-Database into package manager console 

// thirdly: test api in TodoApi.http

/// The login password of app. It should be at least 8 characters long, with one number and 7 character lowercase. ex: florian1