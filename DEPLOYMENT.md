# Deployment Guide - Music Store Application

This guide walks you through deploying the Music Store application from VS Code to GitHub to Render.

## Prerequisites

Before starting, ensure you have:

- [ ] Visual Studio Code installed
- [ ] .NET 8.0 SDK installed ([Download](https://dotnet.microsoft.com/download))
- [ ] Git installed
- [ ] GitHub account ([Sign up](https://github.com/signup))
- [ ] Render account ([Sign up](https://render.com/))

## Step 1: Local Development in VS Code

### 1.1 Open Project in VS Code

```bash
code MusicStoreApp
```

### 1.2 Install Recommended Extensions

VS Code will prompt you to install recommended extensions:
- C# (Microsoft)
- C# Dev Kit (Microsoft)

### 1.3 Test Locally

**Option A: Using Terminal**
```bash
dotnet restore
dotnet run
```

**Option B: Using VS Code**
- Press `F5` or go to Run → Start Debugging
- The application will open in your browser

**Verify it works:**
- Open `http://localhost:5000`
- Change the language, seed, and likes
- Try both Table and Gallery views
- Click on songs to see details and play audio

## Step 2: Push to GitHub

### 2.1 Create GitHub Repository

1. Go to [GitHub](https://github.com/new)
2. Create a new repository:
   - Name: `music-store-app` (or your choice)
   - Description: "Music store web application with generated songs"
   - Visibility: Public or Private
   - **DO NOT** initialize with README (we already have one)
3. Click "Create repository"

### 2.2 Initialize Local Git Repository

In VS Code terminal:

```bash
# Initialize git
git init

# Add all files
git add .

# Create first commit
git commit -m "Initial commit - Music Store Application"

# Rename branch to main
git branch -M main

# Add remote (replace with your repository URL)
git remote add origin https://github.com/YOUR-USERNAME/music-store-app.git

# Push to GitHub
git push -u origin main
```

**Verify on GitHub:**
- Visit your repository URL
- All files should be visible
- README.md should display

## Step 3: Deploy to Render

### 3.1 Create Render Account

1. Go to [Render](https://render.com/)
2. Sign up with GitHub (recommended for easy integration)

### 3.2 Create New Web Service

1. From Render Dashboard, click **"New +"** → **"Web Service"**

2. **Connect Repository:**
   - If first time: Click "Connect GitHub" and authorize Render
   - Find and select your `music-store-app` repository
   - Click "Connect"

3. **Configure Service:**
   
   **Basic Settings:**
   - Name: `music-store-app` (or your choice, this will be in your URL)
   - Region: Choose closest to your location
   - Branch: `main`
   
   **Build Settings:**
   - Environment: **Docker** (important!)
   - Docker Command: Leave empty (will use Dockerfile)
   
   **Instance Type:**
   - For testing: **Free** (note: spins down after inactivity)
   - For production: Starter ($7/month) or higher
   
4. Click **"Create Web Service"**

### 3.3 Wait for Deployment

- Render will automatically:
  1. Clone your repository
  2. Build the Docker image
  3. Deploy the application
  4. Assign a URL

- **First deployment takes 5-10 minutes**
- Watch the deployment logs in real-time

**Deployment stages:**
```
✓ Cloning repository
✓ Building Docker image
✓ Pushing image to registry
✓ Starting web service
✓ Health check passed
✓ Deploy succeeded
```

### 3.4 Access Your Application

Once deployment succeeds:
- Your app URL: `https://music-store-app-xxxx.onrender.com`
- Click the URL or find it at the top of the Render dashboard

**⚠️ Important for Free Tier:**
- Service spins down after 15 minutes of inactivity
- First request after spin-down takes 30-60 seconds
- Consider upgrading to Starter plan for always-on service

## Step 4: Making Updates

### 4.1 Update Code Locally

1. Make changes in VS Code
2. Test locally with `dotnet run`

### 4.2 Push Updates to GitHub

```bash
# Stage changes
git add .

# Commit with descriptive message
git commit -m "Add feature: lyrics display"

# Push to GitHub
git push
```

### 4.3 Automatic Deployment

- Render automatically detects the push
- Triggers a new deployment
- Usually takes 3-5 minutes
- No manual steps required!

## Step 5: Monitoring and Troubleshooting

### View Logs in Render

1. Go to your service in Render dashboard
2. Click "Logs" tab
3. View real-time application logs

### Common Issues

**Issue: "Failed to build Docker image"**
- Check Dockerfile syntax
- Verify all files are committed to Git
- Check Render build logs for specific error

**Issue: "Service unavailable"**
- Free tier spun down (wait 60s and retry)
- Check health check status in Render dashboard
- Review application logs

**Issue: "Application crashes on startup"**
- Check PORT environment variable is used
- Review Program.cs port configuration
- Check logs for startup errors

**Issue: "Audio not playing"**
- Browser compatibility (use Chrome/Firefox/Edge)
- Check console for errors
- Verify audio endpoints are accessible

### Environment Variables (Optional)

To add environment variables in Render:
1. Go to service → "Environment" tab
2. Click "Add Environment Variable"
3. Common variables:
   - `ASPNETCORE_ENVIRONMENT=Production`
   - `ASPNETCORE_URLS=http://0.0.0.0:$PORT`

## Advanced: Custom Domain

### Add Custom Domain to Render

1. Purchase domain (e.g., Namecheap, GoDaddy)
2. In Render: Service → "Settings" → "Custom Domains"
3. Click "Add Custom Domain"
4. Enter your domain: `music-store.yourdomain.com`
5. Add DNS records to your domain provider:
   ```
   Type: CNAME
   Name: music-store
   Value: music-store-app-xxxx.onrender.com
   ```
6. Wait for DNS propagation (5-60 minutes)
7. Render automatically provisions SSL certificate

## Performance Optimization

### For Production Use

1. **Upgrade Instance Type:**
   - Free tier: Spins down, 512 MB RAM
   - Starter: Always on, 512 MB RAM, $7/mo
   - Standard: 2 GB RAM, $25/mo

2. **Enable CDN/Caching:**
   - Consider Cloudflare in front of Render
   - Cache static assets (CSS, JS)

3. **Optimize Docker Image:**
   - Multi-stage builds (already implemented)
   - Smaller base images if needed

## Continuous Integration (Optional)

### Add GitHub Actions

Create `.github/workflows/test.yml`:

```yaml
name: Test Application

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
```

This runs tests on every push!

## Backup and Recovery

### Backup Your Code

- GitHub already backs up your code
- For extra safety: Fork repository or clone locally

### Database Backup (If Added Later)

- Render provides automatic backups for PostgreSQL
- Configure in database settings

## Cost Summary

### Free Tier Limits:
- ✅ Unlimited deployments
- ✅ 750 hours/month
- ✅ Auto-sleep after 15 min
- ✅ Free SSL certificate
- ❌ Spins down when inactive

### Paid Options:
- Starter: $7/month (always on)
- Standard: $25/month (more resources)
- Pro: $85/month (dedicated)

## Support Resources

- **Render Docs:** https://render.com/docs
- **Render Status:** https://status.render.com/
- **Community Forum:** https://community.render.com/
- **GitHub Issues:** Create in your repository

## Checklist for Successful Deployment

- [ ] Code runs locally without errors
- [ ] All files committed to GitHub
- [ ] Render service created and connected
- [ ] Dockerfile present and correct
- [ ] First deployment succeeded
- [ ] Application accessible via Render URL
- [ ] All features working (table, gallery, audio)
- [ ] Multi-language support verified
- [ ] Seed randomization works
- [ ] Audio playback functional

## Next Steps

After successful deployment:

1. **Share your app:** Send the Render URL to users
2. **Monitor usage:** Check Render analytics
3. **Gather feedback:** Improve based on user input
4. **Add features:** Implement optional requirements
5. **Optimize performance:** Upgrade if needed

---

**Congratulations!** 🎉 Your Music Store application is now live on the internet!

If you encounter any issues, check the troubleshooting section or review the Render logs.
