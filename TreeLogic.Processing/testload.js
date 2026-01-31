import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 800 },  // постепенно увеличиваем до 20 пользователей за 30 сек
        { duration: '1m', target: 800 },   // держим 20 пользователей 1 минуту
        { duration: '30s', target: 5000 },  // увеличиваем до 50 пользователей за 30 сек
        { duration: '1m', target: 5000 },   // держим 50 пользователей 1 минуту
        { duration: '30s', target: 0 },   // постепенно уменьшаем нагрузку
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],  // 95% запросов должны быть быстрее 500мс
        http_req_failed: ['rate<0.01'],    // менее 1% запросов могут завершиться с ошибкой
    },
};

export default function () {
    const url = 'http://localhost:5117/exec';

    const payload = JSON.stringify({
        Name: 'GetUsersByAgeRoutine',
        Parameters: [
            {
                Name: 'RoutineOperand',
                Value: '29'
            }
        ]
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const response = http.post(url, payload, params);

    // Проверяем основные метрики
    check(response, {
        'status is 200': (r) => r.status === 200,
        'response time < 500ms': (r) => r.timings.duration < 500,
        'has response body': (r) => r.body.length > 0,
    });

    sleep(1); // Ждем 1 секунду между запросами
}