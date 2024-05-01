# SWTesting Project 3 - Selenium Test
This is the project for the third major assignment - HK232 Software Testing course.

## Instructions for running the project on Visual Studio for members who want to use C# (or simply want to run the project for reference)

First of all, since this repo was created by Visual Studio 2019, the root directory will be master instead of main.

To run the project locally:

1. Clone the project with the URL using terminal/powershell as usual (do not use Visual Studio's clone project feature).
2. Open the file `SWTesting_Proj3_Selenium_Test_C_Sharp.sln` (Visual Studio's solution management file). Visual Studio will open automatically.
3. Install the libraries (note: relatively heavy, recommended to have at least 500MB free in disk):
  1. In the Visual Studio application, click on the `Selenium_Test_C_Sharp` folder to expand the folder.
  2. Right-click on `References` -> `Manage NuGet Packages...`.
  3. At this point, there will be a notification at the top of the newly opened tab saying `Some NuGet packages are missing...`. Click `Restore` to install automatically.
  4. After installation, you must close Visual Studio and then reopen the file `SWTesting_Proj3_Selenium_Test_C_Sharp.sln`. At this point, the new libraries will be fully loaded and ready to run.
4. To test, just press `F5`. The project will compile and start automatically.

## Testing implementation description

Note: Please **ENSURE** to update this section if there are changes in the future.

### C#

1. When the project finishes loading, a terminal window and a simple UI window with the text `Start Probability Calculator Test` will appear.
2. Simply click on it (the speed may vary depending on your machine) and it will automatically start opening `Chrome` and running the test cases of the `Probability Calculator` feature.
3. After finishing, the `Chrome` window will close automatically. Then close all windows that appear from running the code.
4. At this point, in the Visual Studio `Output` section, `Show output from: Debug` will display lines like `Testcase ...: PASS`.

### Rust

1. `cd` to `Selenium_Test_Rust`.
2. Make sure that the browser driver is running at a known port.
3. Specify the correct browser name and if it's not running on the default port, specify the correct port.

   The supported browsers are:
   * `"chrome"`: default to `9515`.
   * `"firefox"`: default to `4444`.
5. To run test level 0, run `cargo run --bin test_level_0`.
6. To run test level 1, run `cargo run --bin test_level_1`.
7. To run test level 2, run `cargo run --bin test_level_2`.

## Some notes about the project (MUST READ):

* The data input for the test cases of the `Probability Calculator` feature is placed in the file `Selenium_Test/bin/Debug/Testcase_Probability.xlsx`. This file has been configured in `.gitignore` to avoid being ignored, so when cloning the project, make sure to have this file.
* If you intend to continue coding in C# on your current project base, you also must configure `.gitignore` to avoid ignoring your data input file (refer to my setup if needed).
* Create separate data input files for different features, do not write to the same file. Because each feature has different input and output formats, so don't use the same file, it will surely be chaotic and extremely difficult to handle reading the file.
* If you don't use C#, create a new directory named `Selenium_Test_<Language>` and place your tests there.

## Technologies used

* Selenium WebDriver
* Selenium.Chrome.WebDriver
* C# - WPF App (.NET Framework)
* Rust - `thirtyfour` crate
