# Library Catalogue 1.0.0
#### made by Mojiboye Emmanuel

#### A Library Application made with ASP.NET6.0 MVC

## Technologies Used
* Git
* C#
* .NET6.0 Framework
* Asp.Net
* MySql

## Description
_An app to catalog a library's books and let patrons check them out._

## Setup/Installation
1. **Clone the Repository**: Open your Git Bash terminal and run the following command to clone the project:
    ```sh
    git clone _REPOSITORY_NAME_
    ```

2. **Install .NET 6.0 Framework**: Ensure you have the .NET 6.0 Framework installed (used .NET 6.0.402 for this application) on your PC. Make sure you have completed the essential steps to write C# code in the C# REPL Terminal.

3. **Create `appSettings.json`**: After cloning the repository, you need to create an `appSettings.json` file in the root directory of the project. Be sure to create it in the production directory of your project (`LibraryCatalogue.Solution/LibraryCatalogue/appSettings.json`). Use the following template for your `appSettings.json`:

    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Port=3306;database=to_do_list_with_mysqlconnector;uid=[YOUR-USERNAME-HERE];pwd=[YOUR-PASSWORD-HERE];"
        }
    }
    ```

    Replace `[YOUR-USERNAME-HERE]` and `[YOUR-PASSWORD-HERE]` with your actual MySQL username and password.

4. **Build and Run the Application**:
    - If you're not interested in seeing the build messages, run the following command to build and run the application:
      ```sh
      dotnet run
      ```
      This command builds and runs the application for you.

    - If you are interested in seeing the build messages, follow these steps:
        1. **Build the Project**: Run the following command to build the project and add the essential directories to execute the application:
            ```sh
            dotnet build
            ```

        2. **Run the Project**: After building the application, run the following command to see all of the results outputted into the console:
            ```sh
            dotnet run
            ```

## Detected Bugs/ Issues
* No detected bugs

## License 
Licensed under the GNU General Public License

## Contact Info
* _Email: emzzyoluwole@gmail.com_
* _Instagram @Emmanuel.9944_
* _Twitter: @Emzzy241 and Profile Name: Dynasty_
* _Github Username: Emzzy241_
