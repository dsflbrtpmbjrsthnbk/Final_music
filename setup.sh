#!/bin/bash

echo "Music Store App - Setup Script"
echo "==============================="
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found. Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"
echo ""

# Restore dependencies
echo "📦 Restoring NuGet packages..."
dotnet restore

if [ $? -ne 0 ]; then
    echo "❌ Failed to restore packages"
    exit 1
fi

echo "✅ Packages restored successfully"
echo ""

# Build the application
echo "🔨 Building application..."
dotnet build -c Release

if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

echo "✅ Build completed successfully"
echo ""

# Ask user what they want to do
echo "What would you like to do?"
echo "1) Run locally (http://localhost:5000)"
echo "2) Setup Git repository"
echo "3) Build Docker image"
echo "4) Exit"
echo ""
read -p "Enter choice (1-4): " choice

case $choice in
    1)
        echo ""
        echo "🚀 Starting application..."
        echo "   Open http://localhost:5000 in your browser"
        echo "   Press Ctrl+C to stop"
        echo ""
        dotnet run
        ;;
    2)
        echo ""
        read -p "Enter your GitHub repository URL: " repo_url
        
        if [ -z "$repo_url" ]; then
            echo "❌ Repository URL is required"
            exit 1
        fi
        
        echo "📁 Initializing Git repository..."
        git init
        git add .
        git commit -m "Initial commit - Music Store Application"
        git branch -M main
        git remote add origin "$repo_url"
        
        echo ""
        echo "✅ Git repository initialized!"
        echo "   To push to GitHub, run:"
        echo "   git push -u origin main"
        ;;
    3)
        echo ""
        echo "🐳 Building Docker image..."
        docker build -t music-store-app .
        
        if [ $? -eq 0 ]; then
            echo ""
            echo "✅ Docker image built successfully!"
            echo "   To run locally:"
            echo "   docker run -p 5000:5000 -e PORT=5000 music-store-app"
        else
            echo "❌ Docker build failed"
            exit 1
        fi
        ;;
    4)
        echo "Goodbye!"
        exit 0
        ;;
    *)
        echo "❌ Invalid choice"
        exit 1
        ;;
esac
