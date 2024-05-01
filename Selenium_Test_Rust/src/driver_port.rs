use dotenv::dotenv;

pub fn get_driver_port() -> usize {
    dotenv().ok();

    let browser = std::env::var("BROWSER").unwrap();
    let port = std::env::var("DRIVER_PORT").unwrap();

    if port != "" {
        port.parse().expect("Invalid driver port specified in .env file")
    } else {
        match browser.to_lowercase().as_str() {
            "chrome" => 9515,
            "firefox" => 4444,
            _ => panic!("Unrecognized browser"),
        }
    }
}
