import { NextRequest, NextResponse } from 'next/server';

const API_URL = process.env.API_URL || 'http://localhost:5078/api';

export async function GET() {
  try {
    const response = await fetch(`${API_URL}/authors`);
    
    if (!response.ok) {
      throw new Error(`Error fetching authors: ${response.statusText}`);
    }
    
    const data = await response.json();
    return NextResponse.json(data);
  } catch (error) {
    console.error('Error in authors API route:', error);
    return NextResponse.json(
      { error: 'Failed to fetch authors' },
      { status: 500 }
    );
  }
}
