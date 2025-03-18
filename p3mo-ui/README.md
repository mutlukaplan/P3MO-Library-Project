# Library Management System

A modern web application for managing a book library with visualization capabilities, built with Next.js 14 and .NET 9.

## Features

- Browse, search, and filter the book collection
- View detailed information about each book
- Add, edit, and remove books from the library
- Visualize library data with interactive charts
- Generate PDF reports of both list and detailed views
- Responsive design for desktop and mobile

## Tech Stack

### Frontend
- **Next.js 14** with App Router and Server Components
- **TailwindCSS** with ShadCN UI components
- **React Hook Form** with Zod validation
- **Highcharts** for data visualization
- **Backend-for-Frontend (BFF)** pattern using Next.js route handlers
- **Server-Side Rendering (SSR)** where applicable

### Backend
- **.NET 9.0** API
- **Entity Framework Core** with Code First approach
- **SQL Server** database
- **Playwright** for PDF generation

## Prerequisites

- [Node.js](https://nodejs.org/en/) 18.x or higher
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Git](https://git-scm.com/)

## Project Setup

### Clone the Repository

```bash
git https://github.com/mutlukaplan/P3MO-Library-Project.git
cd P3MO-Library-Project
```

### Backend Setup

1. **Navigate to the backend directory**:
   ```bash
   cd P3MO.Api
   ```

2. **Restore .NET dependencies**:
   ```bash
   dotnet restore
   ```

3. **Configure the database connection**:
   Open `appsettings.json` and update the `ConnectionStrings:DefaultConnection` with your SQL Server connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=p3mo-dev-mutlu.database.windows.net;Initial Catalog=db-mutlu;Persist Security Info=True;User ID=sqladmin;Password=***********;Trust Server Certificate=True"
   }
   ```

4. **Install Entity Framework Core tools** (if not already installed):
   ```bash
   dotnet tool install --global dotnet-ef
   ```

5. **Create the database and apply migrations**:
   ```bash
   dotnet ef migrations add InitialCreate --project ./../P3MO.Repository/P3MO.Repository.csproj --startup-project ./../P3MO.Api/P3MO.Api.csproj
   dotnet ef database update --project ./../P3MO.Repository/P3MO.Repository.csproj --startup-project ./../P3MO.Api/P3MO.Api.csproj
   ```

6. **Install Playwright browsers** (required for PDF generation):
   ```bash
   # Install the Playwright CLI tool
   dotnet tool install --global Microsoft.Playwright.CLI
   
   # Install the Chromium browser
   playwright install chromium
   ```

7. **Run the backend API**:
   ```bash
   dotnet run
   ```
   The API will start running at `http://localhost:5078`.

### Frontend Setup

1. **Navigate to the frontend directory**:
   ```bash
   cd frontend
   ```

2. **Install Node.js dependencies**:
   ```bash
   npm install
   ```

3. **Create necessary configuration files**:

   **Create tailwind.config.js**:
   ```bash
   cat > tailwind.config.js << 'EOL'
   /** @type {import('tailwindcss').Config} */
   module.exports = {
     darkMode: ["class"],
     content: [
       './pages/**/*.{ts,tsx}',
       './components/**/*.{ts,tsx}',
       './app/**/*.{ts,tsx}',
       './src/**/*.{ts,tsx}',
     ],
     theme: {
       container: {
         center: true,
         padding: "2rem",
         screens: {
           "2xl": "1400px",
         },
       },
       extend: {
         colors: {
           border: "hsl(var(--border))",
           input: "hsl(var(--input))",
           ring: "hsl(var(--ring))",
           background: "hsl(var(--background))",
           foreground: "hsl(var(--foreground))",
           primary: {
             DEFAULT: "hsl(var(--primary))",
             foreground: "hsl(var(--primary-foreground))",
           },
           secondary: {
             DEFAULT: "hsl(var(--secondary))",
             foreground: "hsl(var(--secondary-foreground))",
           },
           destructive: {
             DEFAULT: "hsl(var(--destructive))",
             foreground: "hsl(var(--destructive-foreground))",
           },
           muted: {
             DEFAULT: "hsl(var(--muted))",
             foreground: "hsl(var(--muted-foreground))",
           },
           accent: {
             DEFAULT: "hsl(var(--accent))",
             foreground: "hsl(var(--accent-foreground))",
           },
           popover: {
             DEFAULT: "hsl(var(--popover))",
             foreground: "hsl(var(--popover-foreground))",
           },
           card: {
             DEFAULT: "hsl(var(--card))",
             foreground: "hsl(var(--card-foreground))",
           },
         },
         borderRadius: {
           lg: "var(--radius)",
           md: "calc(var(--radius) - 2px)",
           sm: "calc(var(--radius) - 4px)",
         },
         keyframes: {
           "accordion-down": {
             from: { height: 0 },
             to: { height: "var(--radix-accordion-content-height)" },
           },
           "accordion-up": {
             from: { height: "var(--radix-accordion-content-height)" },
             to: { height: 0 },
           },
         },
         animation: {
           "accordion-down": "accordion-down 0.2s ease-out",
           "accordion-up": "accordion-up 0.2s ease-out",
         },
       },
     },
     plugins: [require("tailwindcss-animate")],
   }
   EOL
   ```

   **Create postcss.config.js**:
   ```bash
   cat > postcss.config.js << 'EOL'
   module.exports = {
     plugins: {
       tailwindcss: {},
       autoprefixer: {},
     },
   }
   EOL
   ```

   **Create next.config.mjs**:
   ```bash
   cat > next.config.mjs << 'EOL'
   /** @type {import('next').NextConfig} */
   const nextConfig = {
     reactStrictMode: true,
     images: {
       domains: ['covers.openlibrary.org', 'images.unsplash.com'],
     },
   };

   export default nextConfig;
   EOL
   ```

   **Create lib/utils.ts**:
   ```bash
   mkdir -p lib
   cat > lib/utils.ts << 'EOL'
   import { type ClassValue, clsx } from "clsx";
   import { twMerge } from "tailwind-merge";

   export function cn(...inputs: ClassValue[]) {
     return twMerge(clsx(inputs));
   }
   EOL
   ```

4. **Set up ShadCN UI components**:
   ```bash
   # Install the ShadCN CLI
   npm install -D shadcn-ui --legacy-peer-deps
   
   # Initialize ShadCN
   npx shadcn@latest init
   ```
   
   During initialization, answer the prompts:
   - Would you like to use TypeScript? **Yes**
   - Which style would you like to use? **Default**
   - Which color would you like to use as base color? **Slate**
   - Where is your tailwind.config.js located? **tailwind.config.js**
   - Where are your global CSS styles located? **app/globals.css**
   - Do you want to use CSS variables? **Yes**
   - Where is your components folder? **components**
   - Do you want to use React Server Components? **Yes**
   - Would you like to include the tailwindcss-animate package? **Yes**

5. **Install required ShadCN components**:
   ```bash
   npx shadcn@latest add button --yes
   npx shadcn@latest add card --yes
   npx shadcn@latest add input --yes
   npx shadcn@latest add textarea --yes
   npx shadcn@latest add select --yes
   npx shadcn@latest add form --yes
   npx shadcn@latest add table --yes
   npx shadcn@latest add dropdown-menu --yes
   npx shadcn@latest add alert-dialog --yes
   npx shadcn@latest add sonner --yes
   ```

6. **Install additional packages for charts and forms**:
   ```bash
   npm install highcharts highcharts-react-official
   npm install sonner
   npm install react-hook-form @hookform/resolvers zod
   npm install lucide-react
   npm install clsx tailwind-merge
   ```

7. **Create a `.env.local` file** in the frontend root directory:
   ```
   API_URL=http://localhost:5078/api
   ```

8. **Start the development server**:
   ```bash
   npm run dev
   ```
   The frontend will start running at `http://localhost:3000`.

## Project Structure

### Backend Structure

```
  /P3MO.Api
    /Controllers
      BooksController.cs
      AuthorsController.cs
      GenresController.cs
      PdfController.cs
    /Models
      Book.cs
      Author.cs
      Genre.cs
    /DTOs
      BookDTO.cs
    /Data
      LibraryDbContext.cs
      /Migrations
    Program.cs
    appsettings.json
```

### Frontend Structure

```
/p3mo-ui
  /app
    /api                 # BFF API handlers
      /books
        /[id]
          route.ts
        route.ts
        /by-genre
          route.ts
      /pdf
        /books
          route.ts
    /books               # Page routes
      /[id]
        page.tsx
      /create
        page.tsx
      /edit
        /[id]
          page.tsx
      page.tsx
    globals.css
    layout.tsx
    page.tsx
  /components
    /books
      BookList.tsx
      BookDetail.tsx
      BookForm.tsx
      BookChart.tsx
    /ui                  # ShadCN UI components
  /lib
    /api
      booksApi.ts        # Frontend API client
    /validations
      book.ts            # Zod validation schemas
    utils.ts             # Utility functions
  next.config.mjs
  tailwind.config.js
  postcss.config.js
  package.json
```

## Database Schema

The application uses a normalized relational database with the following tables:

- **Books**
  - Id (PK)
  - Title
  - PublicationYear
  - ISBN
  - CoverImageUrl
  - Description
  - PageCount
  - AuthorId (FK)
  - GenreId (FK)
  - CreatedAt
  - UpdatedAt

- **Authors**
  - Id (PK)
  - FirstName
  - LastName
  - Biography
  - BirthDate

- **Genres**
  - Id (PK)
  - Name
  - Description

## Using the Application

Once both the frontend and backend are running, you can use the application to:

1. **View Books**: The application opens directly to the books list page with a chart showing distribution by genre.

2. **Add a Book**: Click the "Add Book" button to create a new book entry.

3. **View Book Details**: Click on a book title to see detailed information.

4. **Edit/Delete Books**: Use the actions menu to edit or delete books.

5. **Generate PDFs**: Click the "Print List" button on the books list or the "Print" button on a book detail page to generate a PDF report.

## Troubleshooting

### Common Issues

1. **Database Connection Errors**:
   - Verify your connection string in `appsettings.json`
   - Ensure SQL Server is running
   - Check Windows Authentication or SQL Server credentials

2. **PDF Generation Issues**:
   - If you see a "Playwright browser not found" error, run the Playwright installation commands again:
     ```bash
     dotnet tool install --global Microsoft.Playwright.CLI
     playwright install chromium
     ```

3. **ShadCN Component Errors**:
   - Make sure all required ShadCN components are installed
   - Ensure the `utils.ts` file with the `cn` function exists
   - Verify your tailwind.config.js follows ShadCN requirements
   - Try running with `--legacy-peer-deps` flag during installation

4. **Entity Framework Migrations Errors**:
   - If you get errors about dynamic values in migrations:
     ```bash
     dotnet ef migrations remove
     ```
     Then fix the code to use static dates instead of DateTime.UtcNow in seed data, and create a new migration.

## Adding New Features

### Adding a New Entity

1. Create the entity model in the backend (Project uses different location than it's initial startup project for context file)
2. Add it to the DbContext
3. Create DTOs for the entity
4. Create a migration: `dotnet ef migrations add InitialCreate --project ./../P3MO.Repository/P3MO.Repository.csproj --startup-project ./../P3MO.Api/P3MO.Api.csproj`
5. Update the database: `dotnet ef database update --project ./../P3MO.Repository/P3MO.Repository.csproj --startup-project ./../P3MO.Api/P3MO.Api.csproj`
6. Create controllers for API endpoints
7. Create frontend API clients and components

## Deployment

### Backend Deployment

- Publish the API to Azure App Service or any .NET hosting provider:
  ```bash
  dotnet publish -c Release
  ```
- Set up a production database
- Configure environment variables for connection strings
- Ensure Playwright browser installation in the deployment environment

### Frontend Deployment

- Build the Next.js application:
  ```bash
  npm run build
  ```
- Deploy to Vercel, Netlify, or any other Next.js-compatible hosting
- Set environment variables for the production API URL

## License

This project is licensed under the MIT License - see the LICENSE file for details.
