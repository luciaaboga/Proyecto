window.downloadEditedImage = async (imageUrl, stickers, textElements, filters, transformations, fileName) => {
    return new Promise((resolve) => {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        const img = new Image();
        img.crossOrigin = 'anonymous';
        img.onload = () => {
            canvas.width = img.width;
            canvas.height = img.height;

            ctx.save();

            ctx.translate(canvas.width / 2, canvas.height / 2);
            ctx.rotate(transformations.rotation * Math.PI / 180);

            if (transformations.flipHorizontal) {
                ctx.scale(-1, 1);
            }
            if (transformations.flipVertical) {
                ctx.scale(1, -1);
            }

            ctx.translate(-canvas.width / 2, -canvas.height / 2);

            ctx.filter = `brightness(${filters.brightness}%) contrast(${filters.contrast}%) saturate(${filters.saturation}%)`;

            ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

            ctx.filter = 'none';

            ctx.restore();

            stickers.forEach(sticker => {
                ctx.save();
                ctx.translate(sticker.x, sticker.y);
                ctx.scale(sticker.scale, sticker.scale);
                ctx.rotate(sticker.rotation * Math.PI / 180);

                if (sticker.content.startsWith('http')) {
                    const stickerImg = new Image();
                    stickerImg.src = sticker.content;
                    ctx.drawImage(stickerImg, -25, -25, 50, 50); 
                } else {
                    ctx.font = '48px Arial';
                    ctx.textAlign = 'center';
                    ctx.textBaseline = 'middle';
                    ctx.fillText(sticker.content, 0, 0);
                }

                ctx.restore();
            });

            textElements.forEach(textElement => {
                ctx.save();

                ctx.translate(textElement.x, textElement.y);
                ctx.scale(textElement.scale, textElement.scale);
                ctx.rotate(textElement.rotation * Math.PI / 180);

                ctx.fillStyle = textElement.color;
                ctx.font = `${textElement.isBold ? 'bold' : 'normal'} ${textElement.isItalic ? 'italic' : 'normal'} ${textElement.fontSize}px ${textElement.fontFamily}`;
                ctx.textAlign = 'left';
                ctx.textBaseline = 'top';
                ctx.fillText(textElement.text, 0, 0);

                ctx.restore();
            });

            const dataUrl = canvas.toDataURL('image/png');
            const link = document.createElement('a');
            link.href = dataUrl;
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            resolve(dataUrl);
        };

        img.src = imageUrl;
    });
};