# TodoApp - ASP.NET Core MVC Todo List Application

A modern, responsive todo list application built with ASP.NET Core 8 MVC and Entity Framework Core with an in-memory database.

## Features

- ✅ Create, read, update, and delete todo items
- 🎯 Priority levels (High, Medium, Low)
- 📊 Task status tracking (Pending/Completed)
- 📱 Responsive design with Bootstrap 5
- 🔍 Filter tasks by status and priority
- 📈 Task statistics dashboard
- ⚡ In-memory database for quick development

## Technologies Used

- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM with In-Memory database
- **Bootstrap 5** - CSS framework for responsive design
- **Font Awesome** - Icons
- **jQuery** - Client-side validation and interactions

## Project Structure

```
TodoApp/
├── Controllers/
│   ├── HomeController.cs      # Home page controller
│   └── TodoController.cs      # Main todo CRUD operations
├── Models/
│   └── TodoItem.cs           # Todo item entity model
├── Data/
│   └── TodoContext.cs        # Entity Framework context
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml    # Main layout template
│   │   └── _ValidationScriptsPartial.cshtml
│   └── Todo/
│       ├── Index.cshtml      # Todo list view
│       ├── Create.cshtml     # Create new todo
│       ├── Edit.cshtml       # Edit existing todo
│       ├── Details.cshtml    # Todo details view
│       └── Delete.cshtml     # Delete confirmation
├── Properties/
│   └── launchSettings.json   # Development settings
├── appsettings.json          # Application configuration
└── Program.cs                # Application entry point
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd copilot-demo
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Open your browser and navigate to:**
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`

The application will automatically create an in-memory database with sample data when it starts.

## Key Features Explained

### Todo Item Model
- **Title**: Required field with 200 character limit
- **Description**: Optional field with 1000 character limit
- **Priority**: Enum (Low, Medium, High)
- **Status**: Boolean indicating completion
- **Timestamps**: Creation and completion dates

### Database Context
- Uses Entity Framework Core with In-Memory database
- Includes data seeding with sample todo items
- Configured with proper indexing for performance
- Proper data validation and constraints

### Controller Features
- Full CRUD operations (Create, Read, Update, Delete)
- Task filtering by status and priority
- Toggle completion status
- Comprehensive error handling and logging
- Data validation with model binding

### UI/UX Features
- Modern, responsive design
- Color-coded priority indicators
- Interactive task completion toggle
- Statistics dashboard
- Filter and search capabilities
- Success/error message notifications

## Development Notes

### Error Handling
- Comprehensive exception handling in controllers
- Logging for debugging and monitoring
- User-friendly error messages
- Validation error display

### Security
- Anti-forgery tokens for form submissions
- Input validation and sanitization
- Model binding protection

### Performance
- Efficient database queries
- Proper indexing on frequently queried fields
- Asynchronous operations where appropriate

## Future Enhancements

Potential improvements for this application:
- User authentication and authorization
- Due dates and reminders
- Categories and tags
- File attachments
- Search functionality
- Data export capabilities
- Real database integration (SQL Server, PostgreSQL, etc.)
- API endpoints for mobile app integration

## Contributing

Feel free to submit issues and enhancement requests!

## License

This project is open source and available under the [MIT License](LICENSE).
