# Music Store Web Application

A single-page web application that simulates a music store by generating fake song information with realistic covers and playable audio.

## Features

- **Multi-language Support**: English (USA), Russian (Russia)
- **Seeded Random Generation**: Reproducible data based on 64-bit seed values
- **Dynamic Likes System**: Configurable average likes (0-10) with probabilistic distribution
- **Two View Modes**:
  - Table View with pagination and expandable rows
  - Gallery View with infinite scrolling
- **Generated Content**:
  - Song titles, artists, albums, and genres in selected language
  - Album covers with gradient backgrounds and text
  - Playable audio clips with musical structure (chords, melody, harmony)
  - Review text snippets

## Technology Stack

- **Backend**: ASP.NET Core 8.0 (C#)
- **Frontend**: Vanilla JavaScript, HTML5, CSS3
- **Libraries**:
  - Bogus - Fake data generation
  - NAudio - Audio generation
  - ImageSharp - Image generation

## Local Development

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio Code or Visual Studio 2022

### Setup Instructions

1. **Clone the repository**:
   ```bash
   git clone <your-repo-url>
   cd MusicStoreApp
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

4. **Open in browser**:
   Navigate to `http://localhost:5000` or `https://localhost:5001`

## Deployment to Render

### Prerequisites

- GitHub account
- Render account (free tier available)

### Deployment Steps

1. **Push to GitHub**:
   ```bash
   git init
   git add .
   git commit -m "Initial commit"
   git branch -M main
   git remote add origin <your-github-repo-url>
   git push -u origin main
   ```

2. **Create Render Web Service**:
   - Go to [Render Dashboard](https://dashboard.render.com/)
   - Click "New +" → "Web Service"
   - Connect your GitHub repository
   - Configure the service:
     - **Name**: music-store-app (or your choice)
     - **Environment**: Docker
     - **Region**: Choose closest to your users
     - **Branch**: main
     - **Instance Type**: Free (or higher for better performance)
   - Click "Create Web Service"

3. **Wait for deployment**:
   - Render will automatically build and deploy your application
   - First deployment takes 5-10 minutes
   - You'll get a URL like: `https://music-store-app.onrender.com`

### Environment Variables (Optional)

No environment variables are required for basic operation. The application uses the `PORT` environment variable automatically set by Render.

## Usage Guide

### Toolbar Controls

1. **Region**: Select language for generated content
2. **Seed**: Enter a 64-bit number or click "Random seed"
3. **Average Likes**: Set value between 0-10 (decimals allowed)
4. **View**: Switch between Table and Gallery modes

### Table View

- Click any row to expand and see:
  - Album cover image
  - Audio player
  - Review text
- Click again to collapse
- Use Prev/Next buttons for pagination

### Gallery View

- Scroll down to automatically load more songs
- Each card shows:
  - Cover with gradient background
  - Song metadata
  - Review snippet
  - Audio player

## Technical Details

### Seed System

- User seed + page number → consistent data generation
- Changing seed regenerates all content
- Same seed always produces same results

### Likes Generation

- Fractional values use probabilistic distribution
- Example: 3.7 average = 70% get 4 likes, 30% get 3 likes
- Changing likes only affects like counts, not song data

### Audio Generation

- Real synthesized music (not random noise)
- Uses chord progressions and musical scales
- Includes melody, harmony, and bass tracks
- ADSR envelope for natural sound

### Cover Generation

- Procedural gradient backgrounds
- Rendered text with song title and artist
- Unique colors based on seed

## Architecture

- **Server-side Generation**: All data generated in memory on server
- **No Database**: Uses algorithmic generation instead
- **RESTful API**: JSON endpoints for data fetching
- **Stateless**: No session storage required

## Performance Considerations

- Songs generated on-demand per request
- Covers cached in browser via base64
- Audio streamed directly from server
- Infinite scroll loads 20 items at a time

## Browser Compatibility

- Chrome/Edge 90+
- Firefox 88+
- Safari 14+
- Mobile browsers supported

## License

Educational project - MIT License

## Credits

Built for Task #5 - Web Application Development Course
