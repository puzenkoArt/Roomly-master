/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
const BASE_URL = 'http://localhost:7000/gateway';

function wait(delay: number) {
  return new Promise(resolve => {
    setTimeout(resolve, delay);
  });
}

type RequestMethod = 'GET' | 'POST' | 'PATCH' | 'DELETE' | 'PUT';

function request<T>(
  url: string,
  method: RequestMethod = 'GET',
  data: any = null,
): Promise<T> {
  const options: RequestInit = { method };

  if (data) {
    options.body = JSON.stringify(data);
    options.headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
  }

  const token = localStorage.getItem('token');
  if (token) {
    options.headers = {
      ...options.headers,
      'Authorization': `Bearer ${token}`,
    };
  }

  return wait(300)
    .then(() => fetch(BASE_URL + url, options))
    .then(async (response) => {
      const text = await response.text();

      try {
        const parsed = JSON.parse(text);
        
        if (!response.ok) {
          console.error("Request error:", parsed);
          throw new Error(JSON.stringify(parsed));
        }

        return parsed;
      } catch (error: any) {
        if (!response.ok) {
          throw new Error(text || "Serv error");
        }
        return text as unknown as T;
      }
    });
}

export const client = {
  get: <T>(url: string) => request<T>(url),
  post: <T>(url: string, data?: any) => request<T>(url, 'POST', data),
  patch: <T>(url: string, data: any) => request<T>(url, 'PATCH', data),
  put: <T>(url: string, data?: any) => request<T>(url, 'PUT', data),
  delete: (url: string) => request(url, 'DELETE'),
};
