const API = "https://localhost:7193/api";

//  historial (persistente)
let historialRentas = JSON.parse(localStorage.getItem("historial")) || [];

// almacenamiento global de vehículos
let vehiculosGlobal = [];

// Navegación
function mostrarSeccion(seccion) {
    const secciones = ["home", "vehiculos", "about", "formRenta", "historial"];

    secciones.forEach(s => {
        const el = document.getElementById(s);
        if (el) el.classList.add("d-none");
    });

    const actual = document.getElementById(seccion);
    if (actual) actual.classList.remove("d-none");

    if (seccion === "vehiculos") cargarVehiculos();
    if (seccion === "historial") renderHistorial();
}

// INIT
document.addEventListener("DOMContentLoaded", () => {
    mostrarSeccion("home");
});

//  CARGAR VEHÍCULOS
async function cargarVehiculos() {
    try {
        const res = await fetch(`${API}/vehiculos`);
        const data = await res.json();

        vehiculosGlobal = data;

        const container = document.getElementById("vehiculosContainer");
        container.innerHTML = "";

        data.forEach(v => {
            container.innerHTML += `
                <div class="col-md-4 mb-4">
                    <div class="card shadow-sm h-100">

                        <img src="${v.imagenUrl || 'https://via.placeholder.com/400x200'}"
                             class="card-img-top"
                             style="height:200px; object-fit:cover;">

                        <div class="card-body">
                            <h5>${v.marca} ${v.modelo}</h5>

                            <p>💰 $${v.precioPorDia} / día</p>

                            <span class="badge ${v.disponible ? 'bg-success' : 'bg-danger'}">
                                ${v.disponible ? "Disponible" : "Ocupado"}
                            </span>
                        </div>

                        <div class="card-footer bg-white border-0">
                            <button class="btn btn-dark w-100"
                                onclick="verVehiculo(${v.id})">
                                Ver
                            </button>
                        </div>

                    </div>
                </div>
            `;
        });

    } catch (error) {
        console.error(error);
        alert("Error cargando vehículos");
    }
}

//  VER VEHÍCULO (FIX REAL)
function verVehiculo(id) {

    const v = vehiculosGlobal.find(x => x.id === id);

    if (!v) {
        console.error("Vehículo no encontrado:", id);
        alert("Vehículo no encontrado");
        return;
    }

    // guardar seleccionado
    window.vehiculoSeleccionado = {
        id: v.id,
        marca: v.marca,
        modelo: v.modelo,
        precio: v.precioPorDia
    };

    // título
    document.getElementById("modalTitulo").textContent =
        `${v.marca} ${v.modelo}`;

    document.getElementById("modalPrecio").textContent =
        `$${v.precioPorDia} / día`;

    // ⚠️ FIX IMPORTANTE: este elemento NO existe en tu HTML
    const estadoEl = document.getElementById("modalEstado");
    if (estadoEl) {
        estadoEl.textContent = v.disponible ? "Disponible" : "Ocupado";
    }

    // carrusel
    const carousel = document.getElementById("carouselInner");
    carousel.innerHTML = "";

    let imagenes = [];

    if (v.imagenUrl) imagenes.push(v.imagenUrl);
    if (v.imagenesExtra) imagenes = imagenes.concat(v.imagenesExtra.split(","));

    imagenes.forEach((img, i) => {
        carousel.innerHTML += `
            <div class="carousel-item ${i === 0 ? "active" : ""}">
                <img src="${img}" class="d-block w-100"
                     style="height:300px; object-fit:cover;">
            </div>
        `;
    });

    // botón rentar
    document.getElementById("btnRentar").onclick = () => {

        document.getElementById("vehiculoId").value = v.id;

        const modal = bootstrap.Modal.getInstance(document.getElementById('vehiculoModal'));
        modal.hide();

        mostrarSeccion("formRenta");
    };

    // abrir modal
    new bootstrap.Modal(document.getElementById('vehiculoModal')).show();
}

//  CREAR RENTA
async function crearRenta() {
    const data = {
        vehiculoId: parseInt(document.getElementById("vehiculoId").value),
        clienteNombre: document.getElementById("clienteNombre").value,
        fechaInicio: document.getElementById("fechaInicio").value,
        fechaFin: document.getElementById("fechaFin").value
    };

    try {
        const res = await fetch(`${API}/rentas`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });

        if (!res.ok) throw new Error(await res.text());

        const result = await res.json();

        historialRentas.push({
            vehiculoId: window.vehiculoSeleccionado.id,
            cliente: result.clienteNombre,
            vehiculo: `${window.vehiculoSeleccionado.marca} ${window.vehiculoSeleccionado.modelo}`,
            total: result.total,
            activa: true
        });

        localStorage.setItem("historial", JSON.stringify(historialRentas));

        // limpiar
        document.getElementById("vehiculoId").value = "";
        document.getElementById("clienteNombre").value = "";
        document.getElementById("fechaInicio").value = "";
        document.getElementById("fechaFin").value = "";

        mostrarSeccion("historial");

    } catch (error) {
        console.error(error);
        alert("Error al crear renta");
    }
}

//  HISTORIAL
function renderHistorial() {
    const tabla = document.getElementById("tablaHistorial");
    tabla.innerHTML = "";

    historialRentas.forEach((r, i) => {
        tabla.innerHTML += `
            <tr>
                <td>${r.cliente}</td>
                <td>${r.vehiculo}</td>
                <td>$${r.total}</td>
                <td>
                    ${r.activa
                        ? `<button class="btn btn-warning btn-sm"
                            onclick="devolverVehiculo(${r.vehiculoId}, ${i})">
                            Devolver
                          </button>`
                        : `<span class="badge bg-secondary">Finalizada</span>`
                    }
                </td>
            </tr>
        `;
    });
}

//  DEVOLVER
async function devolverVehiculo(id, index) {
    try {
        await fetch(`${API}/vehiculos/devolver/${id}`, {
            method: "PUT"
        });

        historialRentas[index].activa = false;

        localStorage.setItem("historial", JSON.stringify(historialRentas));

        renderHistorial();
        cargarVehiculos();

    } catch (error) {
        console.error(error);
        alert("Error al devolver vehículo");
    }
}