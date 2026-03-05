(() => {
    const root = document.getElementById("dashboardApp");
    if (!root) {
        return;
    }

    const ROUTE_PREFIX = "/LibraryHub";
    const THEME_KEY = "libraryhub.theme.mode";
    const SIDEBAR_KEY = "libraryhub.sidebar.collapsed";

    const modules = {
        authors: {
            label: "Autores",
            singularLabel: "Autor",
            endpoint: "authors",
            columns: [
                { key: "id", label: "Id", sortable: true, render: (item) => item.id ?? "-" },
                { key: "fullName", label: "Nombre", sortable: true, render: (item) => item.fullName ?? "-" },
                { key: "birthDate", label: "Nacimiento", sortable: true, render: (item) => formatDate(item.birthDate) },
                { key: "city", label: "Ciudad", sortable: true, render: (item) => item.city ?? "-" },
                { key: "email", label: "Correo", sortable: true, render: (item) => item.email ?? "-" },
            ],
            fields: [
                { name: "fullName", label: "Nombre completo", type: "text", required: true, maxLength: 200 },
                { name: "birthDate", label: "Fecha de nacimiento", type: "date", required: true },
                { name: "city", label: "Ciudad", type: "text", required: true, maxLength: 100 },
                { name: "email", label: "Correo", type: "email", required: true, maxLength: 200 },
            ],
            create: () => ({ fullName: "", birthDate: "", city: "", email: "" }),
            payload: (f) => ({ fullName: f.fullName, birthDate: f.birthDate, city: f.city, email: f.email }),
        },
        books: {
            label: "Libros",
            singularLabel: "Libro",
            endpoint: "books",
            columns: [
                { key: "id", label: "Id", sortable: true, render: (item) => item.id ?? "-" },
                { key: "title", label: "Titulo", sortable: true, render: (item) => item.title ?? "-" },
                { key: "year", label: "Año", sortable: true, render: (item) => item.year ?? "-" },
                { key: "genre", label: "Genero", sortable: true, render: (item) => item.genre ?? "-" },
                { key: "pages", label: "Paginas", sortable: true, render: (item) => item.pages ?? "-" },
                { key: "authorFullName", label: "Autor", sortable: true, render: (item) => item.authorFullName ?? "-" },
            ],
            fields: [
                { name: "title", label: "Titulo", type: "text", required: true, maxLength: 250 },
                { name: "year", label: "Año", type: "number", required: true, min: 1, max: 3000 },
                { name: "genre", label: "Genero", type: "text", required: true, maxLength: 100 },
                { name: "pages", label: "Numero de paginas", type: "number", required: true, min: 1, max: 100000 },
                { name: "authorId", label: "Id del autor", type: "number", required: true, min: 1 },
            ],
            create: () => ({ title: "", year: "", genre: "", pages: "", authorId: "" }),
            payload: (f) => ({ title: f.title, year: Number(f.year), genre: f.genre, pages: Number(f.pages), authorId: Number(f.authorId) }),
        },
    };

    const dom = {
        moduleTitle: document.getElementById("moduleTitle"),
        activeModuleLabel: document.getElementById("activeModuleLabel"),
        sidebarToggleButton: document.getElementById("sidebarToggleButton"),
        sidebarToggleIcon: document.getElementById("sidebarToggleIcon"),
        themeToggleButton: document.getElementById("themeToggleButton"),
        themeModeLabel: document.getElementById("themeModeLabel"),
        searchInput: document.getElementById("searchInput"),
        sourceSelect: document.getElementById("sourceSelect"),
        pageSizeSelect: document.getElementById("pageSizeSelect"),
        refreshButton: document.getElementById("refreshButton"),
        newEntityButton: document.getElementById("newEntityButton"),
        tableContainer: document.getElementById("tableContainer"),
        pagerMeta: document.getElementById("pagerMeta"),
        paginationNav: document.getElementById("paginationNav"),
        toastStack: document.getElementById("toastStack"),
        loadingOverlay: document.getElementById("loadingOverlay"),
        loadingOverlayText: document.getElementById("loadingOverlayText"),
        entityModal: document.getElementById("entityModal"),
        entityModalTitle: document.getElementById("entityModalTitle"),
        entityModalHint: document.getElementById("entityModalHint"),
        entityForm: document.getElementById("entityForm"),
        formFields: document.getElementById("formFields"),
        formErrors: document.getElementById("formErrors"),
        saveButton: document.getElementById("saveButton"),
        deleteModal: document.getElementById("deleteModal"),
        deleteModalMessage: document.getElementById("deleteModalMessage"),
        confirmDeleteButton: document.getElementById("confirmDeleteButton"),
    };

    const bs = {
        entityModal: bootstrap.Modal.getOrCreateInstance(dom.entityModal),
        deleteModal: bootstrap.Modal.getOrCreateInstance(dom.deleteModal),
    };

    const state = {
        module: "authors",
        theme: "auto",
        sidebarCollapsed: false,
        debounceId: null,
        pendingDeleteId: null,
        authors: createModuleState(),
        books: createModuleState(),
    };

    function createModuleState() {
        return {
            items: [],
            page: 1,
            size: 5,
            total: 0,
            pages: 0,
            source: "Ef",
            search: "",
            sort: "id",
            dir: "desc",
            loading: false,
            mode: "create",
            editId: null,
            form: {},
        };
    }

    function cfg() {
        return modules[state.module];
    }

    function current() {
        return state[state.module];
    }

    init();

    function init() {
        parseRoute();
        state.theme = localStorage.getItem(THEME_KEY) ?? "auto";
        state.sidebarCollapsed = localStorage.getItem(SIDEBAR_KEY) === "1";
        applyTheme();
        applySidebarState();
        bindEvents();
        renderAll();
        loadCurrent({ syncUrl: true, replaceState: true });
    }

    function bindEvents() {
        document.querySelectorAll("[data-route-link]").forEach((link) => {
            link.addEventListener("click", (event) => {
                event.preventDefault();
                const next = link.getAttribute("data-route-link");
                if (next !== "authors" && next !== "books") {
                    return;
                }

                state.module = next;
                loadCurrent({ syncUrl: true, replaceState: false });
            });
        });

        dom.searchInput.addEventListener("input", () => {
            const m = current();
            m.search = dom.searchInput.value.trim();
            m.page = 1;
            clearTimeout(state.debounceId);
            state.debounceId = window.setTimeout(() => loadCurrent({ syncUrl: true, replaceState: false }), 300);
        });

        dom.sourceSelect.addEventListener("change", () => {
            const m = current();
            m.source = dom.sourceSelect.value;
            m.page = 1;
            loadCurrent({ syncUrl: true, replaceState: false });
        });

        dom.pageSizeSelect.addEventListener("change", () => {
            const m = current();
            m.size = Number(dom.pageSizeSelect.value);
            m.page = 1;
            loadCurrent({ syncUrl: true, replaceState: false });
        });

        dom.refreshButton.addEventListener("click", async () => {
            setButtonLoading(dom.refreshButton, true, "Actualizando...");
            try {
                await loadCurrent({ syncUrl: true, replaceState: true });
            } finally {
                setButtonLoading(dom.refreshButton, false);
            }
        });

        dom.newEntityButton.addEventListener("click", openCreateModal);

        dom.tableContainer.addEventListener("click", (event) => {
            const target = event.target;
            if (!(target instanceof HTMLElement)) {
                return;
            }

            const sortButton = target.closest("button[data-sort-field]");
            if (sortButton instanceof HTMLElement) {
                toggleSort(sortButton.getAttribute("data-sort-field"));
                return;
            }

            const actionButton = target.closest("button[data-row-action]");
            if (!(actionButton instanceof HTMLElement)) {
                return;
            }

            const rowId = Number(actionButton.getAttribute("data-row-id"));
            if (!Number.isInteger(rowId) || rowId <= 0) {
                return;
            }

            const action = actionButton.getAttribute("data-row-action");
            if (action === "edit") {
                openEditModal(rowId);
                return;
            }

            if (action === "delete") {
                openDeleteModal(rowId);
            }
        });

        dom.paginationNav.addEventListener("click", (event) => {
            const target = event.target;
            if (!(target instanceof HTMLElement)) {
                return;
            }

            const pageLink = target.closest("button[data-page]");
            if (!(pageLink instanceof HTMLElement)) {
                return;
            }

            const page = Number(pageLink.getAttribute("data-page"));
            if (!Number.isInteger(page) || page < 1 || page > current().pages || page === current().page) {
                return;
            }

            current().page = page;
            loadCurrent({ syncUrl: true, replaceState: false });
        });

        dom.entityForm.addEventListener("submit", async (event) => {
            event.preventDefault();
            await submitEntity();
        });

        dom.entityForm.addEventListener("input", (event) => {
            const target = event.target;
            if (!(target instanceof HTMLInputElement)) {
                return;
            }

            const field = cfg().fields.find((entry) => entry.name === target.name);
            if (!field) {
                return;
            }

            const validationMessage = validateField(field, target.value);
            setFieldError(field.name, validationMessage, target);
        });

        dom.confirmDeleteButton.addEventListener("click", async () => {
            await confirmDelete();
        });

        dom.themeToggleButton.addEventListener("click", cycleThemeMode);
        dom.sidebarToggleButton?.addEventListener("click", toggleSidebar);

        window.addEventListener("popstate", () => {
            parseRoute();
            renderAll();
            loadCurrent({ syncUrl: false, replaceState: true });
        });

        window.matchMedia("(prefers-color-scheme: dark)").addEventListener("change", () => {
            if (state.theme === "auto") {
                applyTheme();
            }
        });
    }

    function parseRoute() {
        const path = window.location.pathname.toLowerCase();
        state.module = path.includes("/libraryhub/books") ? "books" : "authors";

        const q = new URLSearchParams(window.location.search);
        const m = current();
        m.page = asPositiveInt(q.get("page"), 1);
        m.size = asPositiveInt(q.get("pageSize"), 10);
        m.source = q.get("source") === "Sp" ? "Sp" : "Ef";
        m.search = (q.get("search") ?? "").trim();
        m.sort = q.get("sort") || "id";
        m.dir = q.get("dir") === "asc" ? "asc" : "desc";

        if (!m.form || Object.keys(m.form).length === 0) {
            m.form = cfg().create();
        }
    }

    function asPositiveInt(value, fallback) {
        const n = Number(value);
        return Number.isInteger(n) && n > 0 ? n : fallback;
    }

    function buildRoute() {
        const m = current();
        const q = new URLSearchParams();
        q.set("page", String(m.page));
        q.set("pageSize", String(m.size));
        q.set("source", m.source);
        q.set("sort", m.sort);
        q.set("dir", m.dir);

        if (m.search) {
            q.set("search", m.search);
        }

        return `${ROUTE_PREFIX}/${state.module}?${q.toString()}`;
    }

    async function loadCurrent({ syncUrl, replaceState }) {
        const m = current();

        if (syncUrl) {
            const route = buildRoute();
            if (replaceState) {
                window.history.replaceState({}, "", route);
            } else {
                window.history.pushState({}, "", route);
            }
        }

        m.loading = true;
        renderTable();
        renderPager();

        const q = new URLSearchParams();
        q.set("pageNumber", String(m.page));
        q.set("pageSize", String(m.size));
        q.set("source", m.source);
        if (m.search) {
            q.set("searchTerm", m.search);
        }

        try {
            const response = await apiFetch(`${root.dataset.apiBase}/${cfg().endpoint}/paged?${q.toString()}`, { method: "GET" });
            m.items = Array.isArray(response?.items) ? response.items : [];
            m.total = Number(response?.totalCount) || 0;
            m.page = Number(response?.pageNumber) || m.page;
            m.size = Number(response?.pageSize) || m.size;
            m.pages = Number(response?.totalPages) || (m.total > 0 ? Math.ceil(m.total / m.size) : 0);
        } catch (error) {
            m.items = [];
            m.total = 0;
            m.pages = 0;
            toast(readError(error), "danger");
        } finally {
            m.loading = false;
            renderAll();
        }
    }

    function renderAll() {
        dom.moduleTitle.textContent = cfg().label;
        dom.activeModuleLabel.textContent = cfg().label;
        dom.themeModeLabel.textContent = themeLabel();

        document.querySelectorAll("[data-route-link]").forEach((link) => {
            const active = link.getAttribute("data-route-link") === state.module;
            link.classList.toggle("active", active);
        });

        const m = current();
        dom.searchInput.value = m.search;
        dom.sourceSelect.value = m.source;
        dom.pageSizeSelect.value = String(m.size);

        renderTable();
        renderPager();
    }

    function renderTable() {
        const m = current();
        if (m.loading) {
            dom.tableContainer.innerHTML = '<div class="empty-state"><div class="spinner-border text-primary mb-2" role="status" aria-hidden="true"></div><div>Cargando informacion...</div></div>';
            return;
        }

        if (!m.items.length) {
            dom.tableContainer.innerHTML = '<div class="empty-state">No hay registros con los filtros actuales.</div>';
            return;
        }

        const rows = sortedRows().map((item) => renderRow(item)).join("");
        const headers = cfg().columns.map((column) => renderHeader(column)).join("");

        dom.tableContainer.innerHTML = `
            <div class="table-responsive datagrid-wrapper">
                <table class="table table-hover align-middle table-fixed-header mb-0">
                    <thead>
                        <tr>
                            ${headers}
                            <th class="text-end">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>${rows}</tbody>
                </table>
            </div>`;
    }

    function sortedRows() {
        const m = current();
        const sign = m.dir === "asc" ? 1 : -1;
        return [...m.items].sort((a, b) => {
            const av = normalizeSortValue(a[m.sort]);
            const bv = normalizeSortValue(b[m.sort]);
            if (av < bv) {
                return -1 * sign;
            }
            if (av > bv) {
                return 1 * sign;
            }
            return 0;
        });
    }

    function renderHeader(column) {
        if (!column.sortable) {
            return `<th>${escapeHtml(column.label)}</th>`;
        }

        const m = current();
        const active = m.sort === column.key;
        const icon = active && m.dir === "asc" ? "bi-sort-up" : "bi-sort-down";
        return `<th>
            <button type="button" class="sort-btn ${active ? "active" : ""}" data-sort-field="${column.key}">
                ${escapeHtml(column.label)} <i class="bi ${icon}"></i>
            </button>
        </th>`;
    }

    function renderRow(item) {
        const rowId = Number(item.id);
        const cells = cfg().columns.map((column) => `<td>${escapeHtml(String(column.render(item) ?? ""))}</td>`).join("");

        return `<tr>
            ${cells}
            <td class="text-end">
                <div class="d-inline-flex align-items-center gap-2 table-actions" role="group" aria-label="Acciones de fila">
                    <button type="button" class="btn btn-sm btn-outline-secondary" data-row-action="edit" data-row-id="${rowId}"><i class="bi bi-pencil-square"></i></button>
                    <button type="button" class="btn btn-sm btn-outline-danger" data-row-action="delete" data-row-id="${rowId}"><i class="bi bi-trash3"></i></button>
                </div>
            </td>
        </tr>`;
    }

    function renderPager() {
        const m = current();
        dom.pagerMeta.textContent = m.total === 0
            ? "Sin datos"
            : `Pagina ${m.page} de ${m.pages} · ${m.total} registros`;

        dom.paginationNav.innerHTML = buildPaginationMarkup(m.page, m.pages);
    }

    function buildPaginationMarkup(page, totalPages) {
        if (totalPages <= 0) {
            return "";
        }

        const items = [];
        items.push(paginationItem(page - 1, "Anterior", page <= 1));

        const windowSize = 5;
        const start = Math.max(1, page - 2);
        const end = Math.min(totalPages, start + windowSize - 1);
        const fixedStart = Math.max(1, end - windowSize + 1);

        if (fixedStart > 1) {
            items.push(paginationItem(1, "1", false, page === 1));
            if (fixedStart > 2) {
                items.push(`<li class="page-item disabled"><span class="page-link">...</span></li>`);
            }
        }

        for (let p = fixedStart; p <= end; p += 1) {
            items.push(paginationItem(p, String(p), false, p === page));
        }

        if (end < totalPages) {
            if (end < totalPages - 1) {
                items.push(`<li class="page-item disabled"><span class="page-link">...</span></li>`);
            }

            items.push(paginationItem(totalPages, String(totalPages), false, page === totalPages));
        }

        items.push(paginationItem(page + 1, "Siguiente", page >= totalPages));
        return items.join("");
    }

    function paginationItem(page, text, disabled, active = false) {
        const disabledClass = disabled ? " disabled" : "";
        const activeClass = active ? " active" : "";
        const button = `<button type="button" class="page-link" data-page="${page}">${text}</button>`;
        return `<li class="page-item${disabledClass}${activeClass}">${button}</li>`;
    }

    function normalizeSortValue(value) {
        if (value == null) {
            return "";
        }

        if (typeof value === "number") {
            return value;
        }

        if (typeof value === "string" && value.length >= 10 && value.includes("-")) {
            const dateValue = Date.parse(value);
            if (!Number.isNaN(dateValue)) {
                return dateValue;
            }
        }

        const numeric = Number(value);
        if (!Number.isNaN(numeric) && String(value).trim() !== "") {
            return numeric;
        }

        return String(value).toLowerCase();
    }

    function toggleSort(field) {
        if (!field) {
            return;
        }

        const m = current();
        if (m.sort === field) {
            m.dir = m.dir === "asc" ? "desc" : "asc";
        } else {
            m.sort = field;
            m.dir = "asc";
        }

        renderTable();
        renderPager();
        window.history.replaceState({}, "", buildRoute());
    }

    function openCreateModal() {
        const m = current();
        m.mode = "create";
        m.editId = null;
        m.form = cfg().create();
        renderEntityForm();
        bs.entityModal.show();
    }

    function openEditModal(id) {
        const item = current().items.find((entry) => Number(entry.id) === id);
        if (!item) {
            toast("No fue posible cargar el registro seleccionado.", "danger");
            return;
        }

        const next = cfg().create();
        cfg().fields.forEach((field) => {
            const value = item[field.name];
            if (value == null) {
                next[field.name] = "";
                return;
            }

            if (field.type === "date") {
                next[field.name] = toDateInputValue(value);
                return;
            }

            next[field.name] = String(value);
        });

        const m = current();
        m.mode = "edit";
        m.editId = id;
        m.form = next;
        renderEntityForm();
        bs.entityModal.show();
    }

    function renderEntityForm() {
        const m = current();
        const editMode = m.mode === "edit";

        dom.entityModalTitle.textContent = `${editMode ? "Editar" : "Crear"} ${cfg().singularLabel}`;
        dom.entityModalHint.textContent = editMode
            ? "Actualiza los campos y guarda para persistir cambios."
            : "Completa los campos para registrar un nuevo elemento.";

        dom.formFields.innerHTML = cfg().fields.map((field) => {
            const value = m.form[field.name] ?? "";
            const required = field.required ? "required" : "";
            const maxLength = Number.isFinite(field.maxLength) ? `maxlength="${field.maxLength}"` : "";
            const min = Number.isFinite(field.min) ? `min="${field.min}"` : "";
            const max = Number.isFinite(field.max) ? `max="${field.max}"` : "";

            return `<div class="col-12 col-md-6">
                <label class="form-label" for="field-${field.name}">${escapeHtml(field.label)}</label>
                <input id="field-${field.name}" name="${field.name}" class="form-control" type="${field.type}" value="${escapeHtml(String(value))}" ${required} ${maxLength} ${min} ${max} aria-describedby="field-error-${field.name}" />
                <div class="text-danger small mt-1 field-error" id="field-error-${field.name}" aria-live="polite"></div>
            </div>`;
        }).join("");

        dom.formErrors.textContent = "";
        dom.saveButton.textContent = editMode ? "Actualizar" : "Guardar";
    }

    function openDeleteModal(id) {
        state.pendingDeleteId = id;
        dom.deleteModalMessage.textContent = `¿ Desea eliminar el registro con Id ${id} ?`;
        bs.deleteModal.show();
    }

    async function submitEntity() {
        const values = Object.fromEntries(new FormData(dom.entityForm).entries());
        const validation = validateForm(cfg().fields, values);
        if (!validation.isValid) {
            applyFieldErrors(validation.fieldErrors);
            return;
        }

        clearAllFieldErrors();
        const m = current();
        dom.saveButton.disabled = true;
        setButtonLoading(dom.saveButton, true, m.mode === "edit" ? "Actualizando..." : "Guardando...");
        setLoadingOverlay(true, "Guardando informacion...");
        try {
            const body = cfg().payload(values);
            const editing = m.mode === "edit" && Number.isInteger(m.editId);
            if (editing) {
                await apiFetch(`${root.dataset.apiBase}/${cfg().endpoint}/${m.editId}`, { method: "PUT", body: JSON.stringify(body) });
            } else {
                await apiFetch(`${root.dataset.apiBase}/${cfg().endpoint}`, { method: "POST", body: JSON.stringify(body) });
            }

            bs.entityModal.hide();
            toast(editing ? "Registro actualizado correctamente." : "Registro creado correctamente.", "success");
            await loadCurrent({ syncUrl: true, replaceState: true });
        } catch (error) {
            dom.formErrors.textContent = readError(error);
        } finally {
            dom.saveButton.disabled = false;
            setButtonLoading(dom.saveButton, false);
            setLoadingOverlay(false);
        }
    }

    async function confirmDelete() {
        const id = state.pendingDeleteId;
        if (!Number.isInteger(id)) {
            return;
        }

        dom.confirmDeleteButton.disabled = true;
        setButtonLoading(dom.confirmDeleteButton, true, "Eliminando...");
        setLoadingOverlay(true, "Eliminando registro...");
        try {
            await apiFetch(`${root.dataset.apiBase}/${cfg().endpoint}/${id}`, { method: "DELETE" });
            bs.deleteModal.hide();
            toast("Registro eliminado correctamente.", "success");
            await loadCurrent({ syncUrl: true, replaceState: true });
        } catch (error) {
            toast(readError(error), "danger");
        } finally {
            dom.confirmDeleteButton.disabled = false;
            setButtonLoading(dom.confirmDeleteButton, false);
            setLoadingOverlay(false);
            state.pendingDeleteId = null;
        }
    }

    function setLoadingOverlay(visible, message = "Procesando solicitud...") {
        if (!dom.loadingOverlay || !dom.loadingOverlayText) {
            return;
        }

        dom.loadingOverlayText.textContent = message;
        dom.loadingOverlay.classList.toggle("d-none", !visible);
    }

    function setButtonLoading(button, isLoading, label) {
        if (!(button instanceof HTMLButtonElement)) {
            return;
        }

        if (isLoading) {
            if (!button.dataset.originalHtml) {
                button.dataset.originalHtml = button.innerHTML;
            }

            button.disabled = true;
            button.innerHTML = `<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>${escapeHtml(label)}`;
            return;
        }

        if (button.dataset.originalHtml) {
            button.innerHTML = button.dataset.originalHtml;
            delete button.dataset.originalHtml;
        }

        button.disabled = false;
    }

    function validateForm(fields, values) {
        const fieldErrors = {};

        for (const field of fields) {
            const validationMessage = validateField(field, values[field.name]);
            if (validationMessage) {
                fieldErrors[field.name] = validationMessage;
            }
        }

        return {
            isValid: Object.keys(fieldErrors).length === 0,
            fieldErrors,
        };
    }

    function validateField(field, rawValue) {
        const value = String(rawValue ?? "").trim();

        if (field.required && !value) {
            return `El campo ${field.label} es obligatorio.`;
        }

        if (field.maxLength && value.length > field.maxLength) {
            return `El campo ${field.label} no puede superar ${field.maxLength} caracteres.`;
        }

        if (field.type === "email" && value) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                return "El correo ingresado no tiene un formato valido.";
            }
        }

        if (field.type === "number" && value) {
            const numeric = Number(value);
            if (!Number.isFinite(numeric)) {
                return `El campo ${field.label} debe ser numerico.`;
            }

            if (Number.isFinite(field.min) && numeric < field.min) {
                return `El campo ${field.label} debe ser mayor o igual a ${field.min}.`;
            }

            if (Number.isFinite(field.max) && numeric > field.max) {
                return `El campo ${field.label} debe ser menor o igual a ${field.max}.`;
            }
        }

        return "";
    }

    function applyFieldErrors(fieldErrors) {
        const fields = cfg().fields;
        fields.forEach((field) => {
            const input = dom.entityForm.querySelector(`#field-${field.name}`);
            if (!(input instanceof HTMLInputElement)) {
                return;
            }

            const message = fieldErrors[field.name] ?? "";
            setFieldError(field.name, message, input);
        });
    }

    function setFieldError(fieldName, message, input) {
        const errorContainer = document.getElementById(`field-error-${fieldName}`);
        if (!errorContainer) {
            return;
        }

        errorContainer.textContent = message;
        const hasValue = String(input.value ?? "").trim().length > 0;
        input.classList.toggle("is-invalid", Boolean(message));
        input.classList.toggle("is-valid", !message && hasValue);
    }

    function clearAllFieldErrors() {
        const fields = cfg().fields;
        fields.forEach((field) => {
            const input = dom.entityForm.querySelector(`#field-${field.name}`);
            if (!(input instanceof HTMLInputElement)) {
                return;
            }

            const errorContainer = document.getElementById(`field-error-${field.name}`);
            if (errorContainer) {
                errorContainer.textContent = "";
            }

            input.classList.remove("is-invalid");
            input.classList.remove("is-valid");
        });
    }

    async function apiFetch(url, options) {
        const response = await fetch(url, {
            headers: {
                "Content-Type": "application/json",
            },
            ...options,
        });

        if (!response.ok) {
            throw new Error(await parseError(response));
        }

        if (response.status === 204) {
            return null;
        }

        const contentType = response.headers.get("content-type") ?? "";
        if (!contentType.includes("application/json")) {
            return null;
        }

        return response.json();
    }

    async function parseError(response) {
        const text = await response.text();
        if (!text) {
            return "No fue posible procesar la solicitud.";
        }

        try {
            const payload = JSON.parse(text);
            if (typeof payload.message === "string" && payload.message.trim()) {
                return payload.message.trim();
            }

            if (typeof payload.detail === "string" && payload.detail.trim()) {
                return payload.detail.trim();
            }

            if (payload.errors && typeof payload.errors === "object") {
                const firstError = Object.values(payload.errors)
                    .flat()
                    .find((entry) => typeof entry === "string" && entry.trim());

                if (firstError) {
                    return firstError;
                }
            }

            if (typeof payload.title === "string" && payload.title.trim()) {
                return payload.title.trim();
            }

            return text;
        } catch {
            return text;
        }
    }

    function readError(error) {
        if (error instanceof Error && error.message) {
            return error.message;
        }

        return "Ocurrio un error inesperado.";
    }

    function toast(message, variant) {
        const wrapper = document.createElement("div");
        wrapper.className = `toast align-items-center text-bg-${variant} border-0`;
        wrapper.setAttribute("role", "alert");
        wrapper.setAttribute("aria-live", "assertive");
        wrapper.setAttribute("aria-atomic", "true");
        wrapper.innerHTML = `<div class="d-flex"><div class="toast-body">${escapeHtml(message)}</div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Cerrar"></button></div>`;

        dom.toastStack.appendChild(wrapper);
        const toastInstance = new bootstrap.Toast(wrapper, { delay: 3600 });
        wrapper.addEventListener("hidden.bs.toast", () => wrapper.remove());
        toastInstance.show();
    }

    function cycleThemeMode() {
        if (state.theme === "auto") {
            state.theme = "dark";
        } else if (state.theme === "dark") {
            state.theme = "light";
        } else {
            state.theme = "auto";
        }

        localStorage.setItem(THEME_KEY, state.theme);
        applyTheme();
    }

    function applyTheme() {
        const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;
        const resolved = state.theme === "auto" ? (prefersDark ? "dark" : "light") : state.theme;
        document.documentElement.setAttribute("data-theme", resolved);
        document.documentElement.setAttribute("data-bs-theme", resolved);
        dom.themeModeLabel.textContent = themeLabel();
    }

    function themeLabel() {
        if (state.theme === "auto") {
            return "Auto";
        }

        if (state.theme === "dark") {
            return "Dark";
        }

        return "Light";
    }

    function toggleSidebar() {
        state.sidebarCollapsed = !state.sidebarCollapsed;
        localStorage.setItem(SIDEBAR_KEY, state.sidebarCollapsed ? "1" : "0");
        applySidebarState();
    }

    function applySidebarState() {
        root.classList.toggle("sidebar-collapsed", state.sidebarCollapsed);

        if (dom.sidebarToggleButton) {
            const title = state.sidebarCollapsed ? "Mostrar menu lateral" : "Ocultar menu lateral";
            dom.sidebarToggleButton.setAttribute("aria-expanded", String(!state.sidebarCollapsed));
            dom.sidebarToggleButton.setAttribute("aria-label", title);
            dom.sidebarToggleButton.setAttribute("title", title);
        }

        if (dom.sidebarToggleIcon) {
            dom.sidebarToggleIcon.className = "bi bi-list fs-5";
        }
    }

    function formatDate(raw) {
        if (!raw) {
            return "-";
        }

        const date = new Date(raw);
        if (Number.isNaN(date.getTime())) {
            return String(raw);
        }

        return date.toLocaleDateString("es-CO", {
            year: "numeric",
            month: "2-digit",
            day: "2-digit",
        });
    }

    function toDateInputValue(raw) {
        const date = new Date(raw);
        if (Number.isNaN(date.getTime())) {
            return "";
        }

        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        return `${year}-${month}-${day}`;
    }

    function escapeHtml(value) {
        return value
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#39;");
    }
})();
