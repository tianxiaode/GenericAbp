$(function () {
    abp.log.debug('Index.js initialized!');

    $('#button1').click(()=>{
        let v = Metro.getPlugin('#form1', 'validator');
        v._submit();
    })
});