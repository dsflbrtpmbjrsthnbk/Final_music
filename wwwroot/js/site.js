// State management
let currentPage = 1;
let currentLanguage = 'en-US';
let currentSeed = 1770737616447;
let currentLikes = 3.7;
let currentView = 'table';
let expandedRow = null;
let galleryPage = 1;
let isLoadingGallery = false;

// DOM Elements
const languageSelect = document.getElementById('language');
const seedInput = document.getElementById('seed');
const randomSeedBtn = document.getElementById('randomSeed');
const likesInput = document.getElementById('likes');
const viewModeSelect = document.getElementById('viewMode');
const tableView = document.getElementById('tableView');
const galleryView = document.getElementById('galleryView');
const tableBody = document.getElementById('tableBody');
const galleryGrid = document.getElementById('galleryGrid');
const prevPageBtn = document.getElementById('prevPage');
const nextPageBtn = document.getElementById('nextPage');
const pageInfo = document.getElementById('pageInfo');
const modal = document.getElementById('songModal');
const modalBody = document.getElementById('modalBody');
const closeModal = document.querySelector('.close');

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    loadSongs();
    attachEventListeners();
});

// Event Listeners
function attachEventListeners() {
    languageSelect.addEventListener('change', (e) => {
        currentLanguage = e.target.value;
        resetView();
    });

    seedInput.addEventListener('input', (e) => {
        const value = parseInt(e.target.value);
        if (!isNaN(value)) {
            currentSeed = value;
            resetView();
        }
    });

    randomSeedBtn.addEventListener('click', () => {
        currentSeed = Math.floor(Math.random() * 9007199254740991);
        seedInput.value = currentSeed;
        resetView();
    });

    likesInput.addEventListener('input', (e) => {
        const value = parseFloat(e.target.value);
        if (!isNaN(value) && value >= 0 && value <= 10) {
            currentLikes = value;
            loadSongs();
        }
    });

    viewModeSelect.addEventListener('change', (e) => {
        currentView = e.target.value;
        switchView();
    });

    prevPageBtn.addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            loadSongs();
        }
    });

    nextPageBtn.addEventListener('click', () => {
        currentPage++;
        loadSongs();
    });

    closeModal.addEventListener('click', () => {
        modal.classList.remove('active');
    });

    window.addEventListener('click', (e) => {
        if (e.target === modal) {
            modal.classList.remove('active');
        }
    });

    // Infinite scroll for gallery
    window.addEventListener('scroll', () => {
        if (currentView === 'gallery') {
            const scrollHeight = document.documentElement.scrollHeight;
            const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            const clientHeight = window.innerHeight;

            if (scrollTop + clientHeight >= scrollHeight - 100 && !isLoadingGallery) {
                loadMoreGallery();
            }
        }
    });
}

// Reset view when parameters change
function resetView() {
    currentPage = 1;
    galleryPage = 1;
    expandedRow = null;
    
    if (currentView === 'gallery') {
        galleryGrid.innerHTML = '';
    }
    
    loadSongs();
}

// Switch between table and gallery view
function switchView() {
    if (currentView === 'table') {
        tableView.classList.add('active');
        galleryView.classList.remove('active');
        loadSongs();
    } else {
        tableView.classList.remove('active');
        galleryView.classList.add('active');
        galleryGrid.innerHTML = '';
        galleryPage = 1;
        loadSongs();
    }
}

// Load songs from API
async function loadSongs() {
    try {
        const pageSize = currentView === 'table' ? 20 : 20;
        const page = currentView === 'table' ? currentPage : galleryPage;
        
        const response = await fetch(
            `/Home/GetSongs?language=${currentLanguage}&seed=${currentSeed}&averageLikes=${currentLikes}&page=${page}&pageSize=${pageSize}`
        );
        
        const data = await response.json();
        
        if (currentView === 'table') {
            renderTable(data.songs);
            updatePagination(data.currentPage);
        } else {
            renderGallery(data.songs);
        }
    } catch (error) {
        console.error('Error loading songs:', error);
    }
}

// Load more songs for gallery infinite scroll
async function loadMoreGallery() {
    if (isLoadingGallery) return;
    
    isLoadingGallery = true;
    galleryPage++;
    
    try {
        const response = await fetch(
            `/Home/GetSongs?language=${currentLanguage}&seed=${currentSeed}&averageLikes=${currentLikes}&page=${galleryPage}&pageSize=20`
        );
        
        const data = await response.json();
        renderGallery(data.songs, true);
    } catch (error) {
        console.error('Error loading more songs:', error);
    } finally {
        isLoadingGallery = false;
    }
}

// Render table view
function renderTable(songs) {
    tableBody.innerHTML = '';
    
    songs.forEach(song => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${song.index}</td>
            <td>${song.title}</td>
            <td>${song.artist}</td>
            <td>${song.album}</td>
            <td>${song.genre}</td>
            <td>${song.likes}</td>
        `;
        
        row.addEventListener('click', () => toggleRowDetails(song, row));
        tableBody.appendChild(row);
    });
}

// Toggle row details
async function toggleRowDetails(song, row) {
    // Close previously expanded row
    if (expandedRow && expandedRow !== row) {
        expandedRow.classList.remove('expanded');
        const prevDetailsRow = expandedRow.nextElementSibling;
        if (prevDetailsRow && prevDetailsRow.classList.contains('details-row')) {
            prevDetailsRow.remove();
        }
    }

    // Toggle current row
    if (row.classList.contains('expanded')) {
        row.classList.remove('expanded');
        const detailsRow = row.nextElementSibling;
        if (detailsRow && detailsRow.classList.contains('details-row')) {
            detailsRow.remove();
        }
        expandedRow = null;
    } else {
        row.classList.add('expanded');
        expandedRow = row;
        
        // Load full details
        const details = await loadSongDetails(song.index);
        
        const detailsRow = document.createElement('tr');
        detailsRow.className = 'details-row active';
        detailsRow.innerHTML = `
            <td colspan="6">
                <div class="song-details">
                    <div class="song-details-content">
                        <img src="data:image/png;base64,${details.coverImageBase64}" 
                             alt="${details.title}" 
                             class="cover-image" />
                        <div class="song-info">
                            <h3>${details.title} — ${details.artist}</h3>
                            <p>${details.album} · ${details.genre} · Likes: ${details.likes}</p>
                            <audio controls>
                                <source src="/Home/GetAudio?index=${song.index}&seed=${currentSeed}" type="audio/wav">
                            </audio>
                            <p class="review-text">${details.reviewText}</p>
                        </div>
                    </div>
                </div>
            </td>
        `;
        
        row.after(detailsRow);
    }
}

// Render gallery view
function renderGallery(songs, append = false) {
    if (!append) {
        galleryGrid.innerHTML = '';
    }
    
    songs.forEach(song => {
        const card = document.createElement('div');
        card.className = 'gallery-card';
        
        // Generate gradient colors from song index
        const hue1 = (song.index * 137.5) % 360;
        const hue2 = (hue1 + 60) % 360;
        
        card.innerHTML = `
            <div class="gallery-cover-placeholder" style="background: linear-gradient(135deg, hsl(${hue1}, 70%, 60%), hsl(${hue2}, 70%, 50%));">
                <div class="gallery-cover-text">
                    <h3>${song.title}</h3>
                    <p>${song.artist}</p>
                </div>
            </div>
            <div class="gallery-info">
                <div class="gallery-title">${song.index}. ${song.title}</div>
                <div class="gallery-artist">${song.artist}</div>
                <div class="gallery-meta">
                    <span class="gallery-album">${song.album} · ${song.genre}</span>
                    <span class="gallery-likes">Likes: ${song.likes}</span>
                </div>
            </div>
            <div class="gallery-details">
                <p class="review-preview">${song.reviewText}</p>
                <audio controls style="width: 100%; height: 30px;">
                    <source src="/Home/GetAudio?index=${song.index}&seed=${currentSeed}" type="audio/wav">
                </audio>
            </div>
        `;
        
        galleryGrid.appendChild(card);
    });
}

// Load song details
async function loadSongDetails(index) {
    try {
        const response = await fetch(
            `/Home/GetSongDetails?index=${index}&seed=${currentSeed}&language=${currentLanguage}&averageLikes=${currentLikes}`
        );
        return await response.json();
    } catch (error) {
        console.error('Error loading song details:', error);
        return null;
    }
}

// Update pagination controls
function updatePagination(page) {
    pageInfo.textContent = `Page ${page}`;
    prevPageBtn.disabled = page === 1;
}
