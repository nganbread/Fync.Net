(function () {
    require.config({
        paths: {
            jquery: 'http://code.jquery.com/jquery-2.1.3',
        }
    });

    define('react', [], function () {
        var react = React;
        //delete React;
         return react;
    });
    define('$', [], function () {
        var jquery = $;
        delete $;
         return jquery;
    });
    define('flux', [], function () { return Flux; });
    define('time', [], function () { return moment; });
})();