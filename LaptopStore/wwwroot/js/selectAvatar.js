document
    .querySelector('.avatar-picker')
    .addEventListener(
        'click',
        function () {
            const avatarOptions = document
                .getElementById('avatar-options');
            avatarOptions.style.display = avatarOptions.style.display === 'block' ? 'none'
                : 'block';
        });

function selectAvatar(avatar) {
    document.getElementById('selected-avatar').src = avatar;
    document.getElementById('avatarSrc').value = avatar;
    document.getElementById('avatar-options').style.display = 'none';
}