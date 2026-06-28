(function (window, document) {
    function debounce(fn, wait) {
        let timer;
        return function () {
            const args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                fn.apply(null, args);
            }, wait);
        };
    }

    function escapeHtml(value) {
        return (value ?? '')
            .toString()
            .replaceAll('&', '&amp;')
            .replaceAll('<', '&lt;')
            .replaceAll('>', '&gt;')
            .replaceAll('"', '&quot;')
            .replaceAll("'", '&#39;');
    }

    function renderMarkdown(markdown) {
        const safe = escapeHtml(markdown || '');
        const blocks = safe.trim().split(/\n\s*\n/).filter(Boolean);

        return blocks.map(function (block) {
            const lines = block.split('\n');

            if (lines.every(function (line) { return line.startsWith('- ') || line.startsWith('* '); })) {
                return '<ul>' + lines.map(function (line) {
                    return '<li>' + formatInline(line.slice(2)) + '</li>';
                }).join('') + '</ul>';
            }

            const heading = lines[0].match(/^(#{1,6})\s+(.+)$/);
            if (heading) {
                const level = Math.min(heading[1].length, 6);
                return `<h${level}>${formatInline(heading[2])}</h${level}>`;
            }

            return '<p>' + formatInline(lines.join('<br />')) + '</p>';
        }).join('');
    }

    function formatInline(text) {
        return text
            .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
            .replace(/\*(.+?)\*/g, '<em>$1</em>')
            .replace(/`(.+?)`/g, '<code>$1</code>')
            .replace(/\[(.+?)\]\((https?:\/\/.+?)\)/g, '<a href="$2" target="_blank" rel="noopener noreferrer">$1</a>');
    }

    function createAdminGrid(config) {
        const state = {
            page: 1,
            pageSize: Number(document.querySelector(config.pageSizeInput)?.value || 10),
            sortBy: config.defaultSortBy || 'id',
            sortDirection: config.defaultSortDirection || 'desc'
        };

        const searchInput = document.querySelector(config.searchInput);
        const pageSizeInput = document.querySelector(config.pageSizeInput);
        const tbody = document.querySelector(config.tableSelector + ' tbody');
        const summary = document.querySelector(config.summarySelector);
        const pagination = document.querySelector(config.paginationSelector);
        const headers = document.querySelectorAll(config.tableSelector + ' [data-sort]');

        async function load() {
            const params = new URLSearchParams({
                page: state.page,
                pageSize: state.pageSize,
                sortBy: state.sortBy,
                sortDirection: state.sortDirection
            });

            if (searchInput?.value) {
                params.set('search', searchInput.value);
            }

            if (typeof config.extraParams === 'function') {
                const extra = config.extraParams() || {};
                Object.keys(extra).forEach(function (key) {
                    if (extra[key]) {
                        params.set(key, extra[key]);
                    }
                });
            }

            const response = await fetch(config.endpoint + '?' + params.toString(), {
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            const data = await response.json();
            tbody.innerHTML = config.tbodyRenderer(data.items || []);
            renderSummary(data);
            renderPagination(data);
            syncSortIndicators();
        }

        function renderSummary(data) {
            const totalItems = data.totalItems || 0;
            if (!summary) return;

            if (totalItems === 0) {
                summary.textContent = 'Nenhum registro encontrado.';
                return;
            }

            const start = ((data.page - 1) * data.pageSize) + 1;
            const end = Math.min(start + data.items.length - 1, totalItems);
            summary.textContent = `Mostrando ${start}-${end} de ${totalItems} registros`;
        }

        function renderPagination(data) {
            if (!pagination) return;

            const totalPages = data.totalPages || 0;
            if (totalPages <= 1) {
                pagination.innerHTML = '';
                return;
            }

            let html = '';
            for (let page = 1; page <= totalPages; page++) {
                html += `<button type="button" class="admin-page-btn ${page === data.page ? 'is-active' : ''}" data-page="${page}">${page}</button>`;
            }

            pagination.innerHTML = html;

            pagination.querySelectorAll('[data-page]').forEach(function (button) {
                button.addEventListener('click', function () {
                    state.page = Number(button.dataset.page);
                    load();
                });
            });
        }

        function syncSortIndicators() {
            headers.forEach(function (header) {
                header.classList.remove('is-sort-asc', 'is-sort-desc');
                if (header.dataset.sort === state.sortBy) {
                    header.classList.add(state.sortDirection === 'asc' ? 'is-sort-asc' : 'is-sort-desc');
                }
            });
        }

        headers.forEach(function (header) {
            header.addEventListener('click', function () {
                const sort = header.dataset.sort;
                if (!sort) return;

                if (state.sortBy === sort) {
                    state.sortDirection = state.sortDirection === 'asc' ? 'desc' : 'asc';
                } else {
                    state.sortBy = sort;
                    state.sortDirection = 'asc';
                }

                state.page = 1;
                load();
            });
        });

        searchInput?.addEventListener('input', debounce(function () {
            state.page = 1;
            load();
        }, 250));

        pageSizeInput?.addEventListener('change', function () {
            state.pageSize = Number(pageSizeInput.value || 10);
            state.page = 1;
            load();
        });

        (config.refreshTriggers || []).forEach(function (selector) {
            document.querySelector(selector)?.addEventListener('change', function () {
                state.page = 1;
                load();
            });
        });

        load();
    }

    window.createAdminGrid = createAdminGrid;
    window.renderMarkdown = renderMarkdown;
    window.escapeHtml = escapeHtml;
})(window, document);
