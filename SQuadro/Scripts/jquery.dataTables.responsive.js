var responsiveTables = {
    updateTable: function ($target) {
        if ($(window).width() < 767) {
            this.setHeaders($target);
        }
    },

    setHeaders: function ($table) {
        $('tr td', $table).each(function () {
            var $this = $(this);
            
            if ($this.is('.group'))
                return;

            var $th = $this.closest('table').find('th').eq($this.index());

            var cellHeader = $th.text().fulltrim();
            if (cellHeader != '')
                $('<span class="cell-header"/>').prependTo($this).text(cellHeader + ': ');
        });
    },

    removeHeaders: function ($table) {
        $('span.cell-header', $table).remove();
    }
}

$(document).ready(function () {
    var switched = false;
    var updateTables = function () {
        if (($(window).width() < 767) && !switched) {
            switched = true;
            $("table").each(function (i, element) {
                responsiveTables.setHeaders($(element));
            });
            return true;
        }
        else if (switched && ($(window).width() > 767)) {
            switched = false;
            $("table").each(function (i, element) {
                responsiveTables.removeHeaders($(element));
            });
        }
    };

    $(window).load(updateTables);
    $(window).on("redraw", function () { switched = false; updateTables(); }); // An event to listen for
    $(window).on("resize", updateTables);
});
