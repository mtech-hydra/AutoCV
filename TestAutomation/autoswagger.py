import requests

BASE_URL = "http://localhost:5000/"

def get_token():
    url = f"http://localhost:5000/api/auth/login"
    payload = {
        "email": "string",
        "password": "string"
    }

    response = requests.post(url, json=payload)
    response.raise_for_status()

    token = response.json()["token"]
    return token


def get_coverletters(token):
    url = f"{BASE_URL}/api/CoverLetters"
    headers = {
        "Authorization": f"Bearer {token}"
    }

    response = requests.get(url, headers=headers)
    response.raise_for_status()

    return response.json()


def main():
    token = get_token()
    data = get_coverletters(token)
    print(data)


if __name__ == "__main__":
    main()
