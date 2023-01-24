$(function () {
    abp.log.debug('Index.js initialized!');

    $('#button1').click(()=>{
        let v = Metro.getPlugin('#anbc', 'dialog');
        v.open();
        //v._submit();
    })
});