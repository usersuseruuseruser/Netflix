﻿import styles from './styles/ContentInfo.module.css';

function ContentInfo({contentData}) {
    const groupedByRole = contentData.personInContent.reduce((acc, person) => {
        if (acc[person.profession]) {
            acc[person.profession].push(person);
        } else {
            acc[person.profession] = [person];
        }
        return acc;
    }, {});
    // должно прийти 11 профессий по каждой роли. если их меньше, то последний не получает , ...
    // если их пришло 11, то последний получает , ...
    // это не гарантия что их больше 11, но хоть что-то
    function printAllPersonsByRole(profession){
        return groupedByRole[profession].map((person, index) => {
            if (groupedByRole[profession].length < 11){
                if (index === (groupedByRole[profession].length - 1)) {
                    return person.name + "";
                } else {
                    return person.name + ", ";
                }
            }
            else{
                if (index === (groupedByRole[profession].length - 1)) {
                    return person.name + ", ...";
                } else {
                    return person.name + ", ";
                }
            }
        });
    }
    function getNoun(number, one, two, five) {
        let n = Math.abs(number);
        n %= 100;
        if (n >= 5 && n <= 20) {
            return five;
        }
        n %= 10;
        if (n === 1) {
            return one;
        }
        if (n >= 2 && n <= 4) {
            return two;
        }
        return five;
    }
    function capitalizeFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }
    return (
        <>
            <div className={styles.container}>
                <div className={styles.posterTeaser}>
                    <img src={contentData.poster} alt="poster" className={styles.poster}/>
                    <div className={styles.trailer}>
                        <h3>{contentData.trailerInfo.name}</h3>
                        <iframe className={styles.trailerEmbed}
                                allowFullScreen={true}
                                src={contentData.trailerInfo.url}>
                        </iframe>
                    </div>
                </div>
                <div className={styles.contentInfo}>
                    <span className={styles.title}>{contentData.title}</span>
                    <span className={styles.description}>{contentData.description}</span>
                    <h2>Детали</h2>
                    <span><strong>Страна:</strong> {contentData.country}</span>
                    <span><strong>Слоган:</strong> {contentData.slogan}</span>
                    <span><strong>Жанры:</strong> {contentData.genres.join(", ")}</span>
                    <span><strong>Тип контента:</strong> {contentData.contentType}</span>
                    <span><strong>Дата выхода:</strong> {contentData.releaseDate}</span>
                    <span><strong>Продолжительность:</strong> {contentData.movieLength} {getNoun(contentData.movieLength,"минута","минуты","минут")}</span>
                    <span><strong>Бюджет:</strong> {contentData.budget.budget} {contentData.budget.currency}</span>
                    <span className={styles.ageRating}><strong>Возрастной рейтинг: </strong>
                        <span>{contentData.ageRating.age}+ ({contentData.ageRating.ageMpaa})</span></span>
                    <span className={styles.ratings}>
                        <strong>Рейтинги:</strong>
                        <span className={styles.ratingImdb}>IMDb: {contentData.ratings.imdb}</span>
                        <span className={styles.ratingKinopoisk}>Кинопоиск: {contentData.ratings.kinopoisk}</span>
                        <span className={styles.ratingLocal}>Локальный: {contentData.ratings.local}</span></span>
                    <h2>В фильме снимались: </h2>
                    <span>{printAllPersonsByRole("актеры")}</span>
                    <h2>Над фильмом также работали</h2>
                    {Object.keys(groupedByRole).map((role) => {
                        if (role === "актеры") {
                            return;
                        }
                        return <span><strong>{capitalizeFirstLetter(role)}</strong> {printAllPersonsByRole(role)}</span>

                    })}
                </div>

            </div>
        </>


    );
}

export default ContentInfo;