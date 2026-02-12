let currentPage = 1;

function getParams() {
    return {
        language: document.getElementById("language").value,
        seed: BigInt(document.getElementById("seed").value),
        averageLikes: parseFloat(document.getElementById("likes").value)
    };
}

async function fetchSongs(page) {
    const params = getParams();
    const url = `/Home/GetSongs?page=${page}&pageSize=20&seed=${params.seed}&language=${params.language}&averageLikes=${params.averageLikes}`;
    const res = await fetch(url);
    return await res.json();
}

async function fetchSongDetails(index) {
    const params = getParams();
    const url = `/Home/GetSongDetails?index=${index}&seed=${params.seed}&language=${params.language}&averageLikes=${params.averageLikes}`;
    const res = await fetch(url);
    return await res.json();
}

async function fetchAudio(index) {
    const params = getParams();
    const url = `/Home/GetAudio?index=${index}&seed=${params.seed}`;
    const res = await fetch(url);
    return await res.arrayBuffer();
}

function createAudioBlob(arrayBuffer) {
    const blob = new Blob([arrayBuffer], { type: 'audio/wav' });
    return URL.createObjectURL(blob);
}

function renderTable(songs) {
    const tbody = document.getElementById("tableBody");
    tbody.innerHTML = "";
    songs.forEach(song => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${song.index}</td>
            <td>${song.title}</td>
            <td>${song.artist}</td>
            <td>${song.album}</td>
            <td>${song.genre}</td>
            <td>${song.likes}</td>
            <td><button class="playBtn" data-index="${song.index}">Play</button></td>
        `;
        tbody.appendChild(tr);
    });
    document.getElementById("pageInfo").textContent = `Page ${currentPage}`;

    // Add play button listener
    document.querySelectorAll(".playBtn").forEach(btn => {
        btn.onclick = async () => {
            const index = btn.dataset.index;
            const audioBuffer = await fetchAudio(index);
            const audioUrl = createAudioBlob(audioBuffer);
            showModal(index, audioUrl);
        };
    });
}

function renderGallery(songs) {
    const grid = document.getElementById("galleryGrid");
    grid.innerHTML = "";
    songs.forEach(song => {
        const div = document.createElement("div");
        div.className = "gallery-item";
        div.innerHTML = `
            <img src="data:image/png;base64,${song.coverImageBase64}" alt="${song.title}" />
            <h4>${song.title}</h4>
            <p>${song.artist}</p>
            <button class="playBtn" data-index="${song.index}">Play</button>
        `;
        grid.appendChild(div);
    });

    document.querySelectorAll("#galleryGrid .playBtn").forEach(btn => {
        btn.onclick = async () => {
            const index = btn.dataset.index;
            const audioBuffer = await fetchAudio(index);
            const audioUrl = createAudioBlob(audioBuffer);
            showModal(index, audioUrl);
        };
    });
}

function showModal(index, audioUrl) {
    const modal = document.getElementById("songModal");
    const modalBody = document.getElementById("modalBody");
    modalBody.innerHTML = `
        <audio controls autoplay>
            <source src="${audioUrl}" type="audio/wav">
            Your browser does not support the audio element.
        </audio>
    `;
    modal.style.display = "block";
}

document.querySelector(".close").onclick = () => {
    document.getElementById("songModal").style.display = "none";
};

document.getElementById("prevPage").onclick = async () => {
    if (currentPage > 1) {
        currentPage--;
        const data = await fetchSongs(currentPage);
        renderTable(data.songs);
    }
};

document.getElementById("nextPage").onclick = async () => {
    currentPage++;
    const data = await fetchSongs(currentPage);
    renderTable(data.songs);
};

document.getElementById("viewMode").onchange = async (e) => {
    const mode = e.target.value;
    document.getElementById("tableView").classList.toggle("active", mode === "table");
    document.getElementById("galleryView").classList.toggle("active", mode === "gallery");

    const data = await fetchSongs(currentPage);
    if (mode === "table") renderTable(data.songs);
    else renderGallery(data.songs);
};

document.getElementById("randomSeed").onclick = () => {
    const seed = BigInt(Date.now());
    document.getElementById("seed").value = seed.toString();
};

document.getElementById("language").onchange = async () => {
    currentPage = 1;
    const data = await fetchSongs(currentPage);
    const mode = document.getElementById("viewMode").value;
    if (mode === "table") renderTable(data.songs);
    else renderGallery(data.songs);
};

document.getElementById("likes").onchange = async () => {
    const data = await fetchSongs(currentPage);
    const mode = document.getElementById("viewMode").value;
    if (mode === "table") renderTable(data.songs);
    else renderGallery(data.songs);
};

// Initial load
(async () => {
    const data = await fetchSongs(currentPage);
    renderTable(data.songs);
})();
