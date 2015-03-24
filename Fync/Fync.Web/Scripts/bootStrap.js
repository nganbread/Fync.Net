(function () {
    require.config({
        paths: {
            jquery: 'http://code.jquery.com/jquery-2.1.3',
        }
    });

    define('react', [], function() { return React; });
    define('flux', [], function () { return Flux; });
    define('time', [], function () { return moment; });
})();