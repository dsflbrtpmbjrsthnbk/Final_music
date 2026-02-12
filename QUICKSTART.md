# Quick Start Guide

Get the Music Store app running in 5 minutes!

## 🚀 Option 1: Run Locally (Fastest)

### Requirements
- .NET 8.0 SDK ([Download](https://dotnet.microsoft.com/download))

### Steps
```bash
# Navigate to project directory
cd MusicStoreApp

# Restore packages
dotnet restore

# Run the application
dotnet run
```

**Open in browser:** http://localhost:5000

That's it! 🎉

---

## 🌐 Option 2: Deploy to Render (For Production)

### Requirements
- GitHub account
- Render account (free)

### Steps

**1. Push to GitHub:**
```bash
git init
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/music-store-app.git
git push -u origin main
```

**2. Deploy on Render:**
- Go to [Render Dashboard](https://dashboard.render.com/)
- New + → Web Service
- Connect your GitHub repository
- Settings:
  - Environment: **Docker**
  - Instance: **Free**
- Click "Create Web Service"
- Wait 5-10 minutes

**Your app is live!** 🌟

URL: `https://music-store-app-xxxx.onrender.com`

---

## 🎵 Using the Application

### Controls
1. **Region:** Select language (English, Russian)
2. **Seed:** Enter number or click "Random seed"
3. **Likes:** Set average 0-10 (e.g., 3.7)
4. **View:** Toggle Table/Gallery mode

### Features
- **Table View:** Click rows to expand details
- **Gallery View:** Scroll for infinite loading
- **Audio:** Click play to hear generated music
- **Covers:** Auto-generated album art

### Tips
- Same seed = same songs
- Changing seed = new songs
- Changing likes = only like counts change
- All content generated in real-time!

---

## 📝 What's Included

✅ Multi-language support (EN, DE, UA)  
✅ Seeded random generation  
✅ Table view with pagination  
✅ Gallery view with infinite scroll  
✅ Generated album covers  
✅ Playable music with chords/melody  
✅ Review text snippets  
✅ Responsive design  
✅ No database required  

---

## 🆘 Troubleshooting

**"dotnet: command not found"**
→ Install .NET 8.0 SDK

**"Port already in use"**
→ Change port: `dotnet run --urls "http://localhost:5001"`

**Audio not playing**
→ Use Chrome, Firefox, or Edge browser

**Render app sleeping**
→ Free tier sleeps after 15 min. Wait 60s on first load.

---

## 📚 Full Documentation

- Complete deployment guide: See `DEPLOYMENT.md`
- Technical details: See `README.md`
- Architecture details: See source code comments

---

## 🎯 Assignment Requirements Met

✅ Single-page application  
✅ Multiple languages  
✅ Custom seed support  
✅ Fractional likes (probabilistic)  
✅ Table view with expandable rows  
✅ Gallery view with infinite scroll  
✅ Server-side generation  
✅ No hardcoded locale data  
✅ Reproducible from seed  
✅ Album covers generated  
✅ Audio generation  
✅ Review text  

**Optional (Bonus):**
- Realistic-looking song data ✅
- Musical chord progressions ✅
- Gradient album covers ✅
- Clean, modern UI ✅

---

**Need help?** Check the logs or open an issue on GitHub.

**Enjoy your music store!** 🎶
